using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Threading.Channels;
using ChoreoApp.Floor.Messages;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;
using MaterialDesignThemes.Maui;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Floor.Behaviors;

public sealed class GestureHandlingBehavior(
    ApplicationStateMachine stateMachine,
    ILogger<FloorCanvasViewModel> logger):
    IBehavior<FloorCanvasViewModel>
{
    private const float TouchPanFactor = 0.5f;
    private readonly Channel<TouchCommand> _touchChannel = Channel.CreateBounded<TouchCommand>(
        new BoundedChannelOptions(1)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });

    private readonly Dictionary<long, SKPoint> _activeTouches = new();

    private Point? _lastHoverPosition;
    private Point? _lastPointerPosition;
    private Point? _lastSingleTouchPosition;
    private SKPoint? _lastTouchCenter;
    private float? _lastTouchDistance;
    private bool _touchPanActive;
    private bool _touchZoomActive;

    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(GestureHandlingBehavior), nameof(FloorCanvasViewModel));
        viewModel.PointerPressedCommand
            .Subscribe(HandlePointerPressed)
            .DisposeWith(disposables);

        viewModel.PointerMovedCommand
            .Subscribe(command => HandlePointerMoved(viewModel, command))
            .DisposeWith(disposables);

        viewModel.PointerReleasedCommand
            .Subscribe(HandlePointerReleased)
            .DisposeWith(disposables);

        viewModel.PointerWheelChangedCommand
            .Subscribe(command => HandlePointerWheelChanged(viewModel, command))
            .DisposeWith(disposables);

        viewModel.TouchCommand
            .Subscribe(command => _touchChannel.Writer.TryWrite(command))
            .DisposeWith(disposables);

        var cancellationTokenSource = new CancellationTokenSource();
        var readerTask = Task.Run(async () => await ProcessTouchCommandsAsync(viewModel, cancellationTokenSource.Token),
            cancellationTokenSource.Token);
        Disposable
            .Create(() =>
            {
                cancellationTokenSource.Cancel();
                _touchChannel.Writer.TryComplete();
                _ = readerTask;
            })
            .DisposeWith(disposables);
    }

    private async Task ProcessTouchCommandsAsync(FloorCanvasViewModel viewModel, CancellationToken cancellationToken)
    {
        var reader = _touchChannel.Reader;
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            TouchCommand? latest = null;
            while (reader.TryRead(out var command))
            {
                latest = command;
            }

            if (latest is null)
            {
                continue;
            }

            RxSchedulers.MainThreadScheduler.Schedule(() => HandleTouch(viewModel, latest));
        }
    }


    private void HandlePointerPressed(PointerPressedCommand command)
    {
        if (stateMachine.State is MovePositionsState or RotateAroundCenterState or ScalePositionsState or ScaleAroundDancerState)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView as SKCanvasView);
        _lastHoverPosition = position;

        if (command.EventArgs.Button != ButtonsMask.Primary || position is null)
        {
            _lastPointerPosition = null;
            return;
        }

        _lastPointerPosition = position.Value;
    }

    private void HandlePointerMoved(FloorCanvasViewModel viewModel, PointerMovedCommand command)
    {
        if (stateMachine.State is MovePositionsState or RotateAroundCenterState or ScalePositionsState or ScaleAroundDancerState)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView as SKCanvasView);
        if (position is null)
        {
            return;
        }

        _lastHoverPosition = position.Value;

        if (_lastPointerPosition is null)
        {
            return;
        }

        var deltaX = position.Value.X - _lastPointerPosition.Value.X;
        var deltaY = position.Value.Y - _lastPointerPosition.Value.Y;

        stateMachine.TryApply(new PanStartedTrigger());
        ApplyTranslation(viewModel, command.CanvasView, deltaX, deltaY);
        _lastPointerPosition = position.Value;
        InvalidateCanvas(viewModel);
    }

    private void HandlePointerReleased(PointerReleasedCommand _)
    {
        if (stateMachine.State is MovePositionsState or RotateAroundCenterState or ScalePositionsState or ScaleAroundDancerState)
        {
            return;
        }

        stateMachine.TryApply(new PanCompletedTrigger());
        _lastPointerPosition = null;
    }

    private void HandlePointerWheelChanged(FloorCanvasViewModel viewModel, PointerWheelChangedCommand command)
    {
        if (stateMachine.State is MovePositionsState or RotateAroundCenterState or ScalePositionsState or ScaleAroundDancerState)
        {
            return;
        }

        stateMachine.TryApply(new ZoomStartedTrigger());
        var zoomFactor = command.Delta > 0 ? 1.1f : 0.9f;
        var zoomCenter = command.Position ?? _lastHoverPosition;
        if (zoomCenter is null)
        {
            return;
        }

        var (dpiScaleX, dpiScaleY) = GetCanvasScale(command.CanvasView);
        var originX = (float)(zoomCenter.Value.X * dpiScaleX);
        var originY = (float)(zoomCenter.Value.Y * dpiScaleY);

        var scaleMatrix = SKMatrix.CreateScale(zoomFactor, zoomFactor, originX, originY);
        ApplyTransformation(viewModel, scaleMatrix);
        InvalidateCanvas(viewModel);
        stateMachine.TryApply(new ZoomCompletedTrigger());
    }

    private void HandleTouch(FloorCanvasViewModel viewModel, TouchCommand command)
    {
        if (stateMachine.State is MovePositionsState or RotateAroundCenterState or ScalePositionsState or ScaleAroundDancerState)
        {
            return;
        }

        var args = command.EventArgs;
        var (dpiScaleX, dpiScaleY) = GetCanvasScale(command.CanvasView);
        if (args.InContact)
        {
            _activeTouches[args.Id] = args.Location;
        }
        else
        {
            _activeTouches.Remove(args.Id);
        }

        if (_activeTouches.Count == 0)
        {
            ResetTouchState();
            return;
        }

        if (_activeTouches.Count == 1)
        {
            if (_touchZoomActive)
            {
                stateMachine.TryApply(new ZoomCompletedTrigger());
                _touchZoomActive = false;
                _lastTouchCenter = null;
                _lastTouchDistance = null;
            }

            HandleSingleTouchPan(viewModel, command.CanvasView, dpiScaleX, dpiScaleY);
            return;
        }

        _lastSingleTouchPosition = null;
        var touchPoints = _activeTouches.Values.Take(2).ToArray();
        var first = new Point(touchPoints[0].X / dpiScaleX, touchPoints[0].Y / dpiScaleY);
        var second = new Point(touchPoints[1].X / dpiScaleX, touchPoints[1].Y / dpiScaleY);

        var center = new Point(
            (first.X + second.X) / 2f,
            (first.Y + second.Y) / 2f);

        var dx = (float)(second.X - first.X);
        var dy = (float)(second.Y - first.Y);
        var distance = MathF.Sqrt(MathF.Pow(dx, 2f) + MathF.Pow(dy, 2f));

        if (!_touchZoomActive)
        {
            _touchZoomActive = true;
            stateMachine.TryApply(new ZoomStartedTrigger());
        }

        if (_lastTouchCenter is { } lastCenterPoint
            && _lastTouchDistance is { } lastDistance
            && lastDistance > 0f)
        {
            var deltaX = (float)((center.X - lastCenterPoint.X) * TouchPanFactor);
            var deltaY = (float)((center.Y - lastCenterPoint.Y) * TouchPanFactor);
            ApplyTranslation(viewModel, command.CanvasView, deltaX, deltaY);

            var scale = distance / lastDistance;
            if (scale is > 0f and < float.PositiveInfinity)
            {
                var originX = (float)(center.X * dpiScaleX);
                var originY = (float)(center.Y * dpiScaleY);
                var scaleMatrix = SKMatrix.CreateScale(scale, scale, originX, originY);
                ApplyTransformation(viewModel, scaleMatrix);
            }

            InvalidateCanvas(viewModel);
        }

        _lastTouchCenter = new SKPoint((float)center.X, (float)center.Y);
        _lastTouchDistance = distance;
        args.Handled = true;
    }

    private void HandleSingleTouchPan(
        FloorCanvasViewModel viewModel,
        ISKCanvasView canvasView,
        float dpiScaleX,
        float dpiScaleY)
    {
        var touchPoint = _activeTouches.Values.First();
        var current = new Point(touchPoint.X / dpiScaleX, touchPoint.Y / dpiScaleY);

        if (_lastSingleTouchPosition is null)
        {
            _lastSingleTouchPosition = current;
            return;
        }

        if (!_touchPanActive)
        {
            _touchPanActive = true;
            stateMachine.TryApply(new PanStartedTrigger());
        }

        var deltaX = current.X - _lastSingleTouchPosition.Value.X;
        var deltaY = current.Y - _lastSingleTouchPosition.Value.Y;

        ApplyTranslation(viewModel, canvasView, deltaX, deltaY);
        _lastSingleTouchPosition = current;
        InvalidateCanvas(viewModel);
    }

    private void ResetTouchState()
    {
        if (_touchPanActive)
        {
            stateMachine.TryApply(new PanCompletedTrigger());
        }

        if (_touchZoomActive)
        {
            stateMachine.TryApply(new ZoomCompletedTrigger());
        }

        _touchPanActive = false;
        _touchZoomActive = false;
        _lastSingleTouchPosition = null;
        _lastTouchCenter = null;
        _lastTouchDistance = null;
    }

    private static void InvalidateCanvas(FloorCanvasViewModel viewModel)
    {
        viewModel.CanvasView?.InvalidateSurface();
    }

    private static void ApplyTranslation(FloorCanvasViewModel viewModel, ISKCanvasView canvasView, double deltaX, double deltaY)
    {
        var (dpiScaleX, dpiScaleY) = GetCanvasScale(canvasView);
        var sx = viewModel.TransformationMatrix.ScaleX;
        var sy = viewModel.TransformationMatrix.ScaleY;

        var translationMatrix = SKMatrix.CreateTranslation(
            (float)(deltaX * dpiScaleX) / sx,
            (float)(deltaY * dpiScaleY) / sy);

        ApplyTransformation(viewModel, translationMatrix);
    }

    private static void ApplyTransformation(FloorCanvasViewModel viewModel, SKMatrix newMatrix)
    {
        var newTransformationMatrix = SKMatrix.Concat(viewModel.TransformationMatrix, newMatrix);
        var scaleX = newTransformationMatrix.ScaleX;
        var scaleY = newTransformationMatrix.ScaleY;

        if (scaleX <= FloorCanvasViewModel.MaxZoomFactor
            && scaleY <= FloorCanvasViewModel.MaxZoomFactor
            && scaleX >= FloorCanvasViewModel.MinZoomFactor
            && scaleY >= FloorCanvasViewModel.MinZoomFactor)
        {
            viewModel.TransformationMatrix = ClampTranslation(viewModel, newTransformationMatrix);
        }
    }

    private static SKMatrix ClampTranslation(FloorCanvasViewModel viewModel, SKMatrix matrix)
    {
        if (!viewModel.HasFloorBounds || viewModel.CanvasSize.Width <= 0 || viewModel.CanvasSize.Height <= 0)
        {
            return matrix;
        }

        var scaleX = matrix.ScaleX;
        var scaleY = matrix.ScaleY;

        var marginX = FloorCanvasViewModel.PanMargin * scaleX;
        var marginY = FloorCanvasViewModel.PanMargin * scaleY;

        var minTransX = -viewModel.FloorBounds.Right * scaleX + marginX;
        var maxTransX = viewModel.CanvasSize.Width - viewModel.FloorBounds.Left * scaleX - marginX;
        var minTransY = -viewModel.FloorBounds.Bottom * scaleY + marginY;
        var maxTransY = viewModel.CanvasSize.Height - viewModel.FloorBounds.Top * scaleY - marginY;

        var clampedTransX = Math.Clamp(matrix.TransX, minTransX, maxTransX);
        var clampedTransY = Math.Clamp(matrix.TransY, minTransY, maxTransY);

        matrix.TransX = clampedTransX;
        matrix.TransY = clampedTransY;
        return matrix;
    }

    private static (float ScaleX, float ScaleY) GetCanvasScale(ISKCanvasView canvasView)
    {
        if (!canvasView.IsValid())
        {
            return (1f, 1f);
        }

        var width = canvasView.Width;
        var height = canvasView.Height;
        var scaleX = canvasView.CanvasSize.Width / (float)width;
        var scaleY = canvasView.CanvasSize.Height / (float)height;
        return (scaleX, scaleY);
    }
}
