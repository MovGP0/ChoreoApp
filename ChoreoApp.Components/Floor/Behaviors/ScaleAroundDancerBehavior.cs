using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Scenes;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;
using MaterialDesignThemes.Maui;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Position = ChoreoApp.Models.PositionModel;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Floor.Behaviors;

public sealed class ScaleAroundDancerBehavior(
    Global.GlobalStateModel globalState,
    ApplicationStateMachine stateMachine,
    TimeProvider timeProvider,
    IVibration vibration,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber,
    ILogger<FloorCanvasViewModel> logger)
    : IBehavior<FloorCanvasViewModel>
{
    private const float PointerMoveThreshold = 6f;
    private const float DoubleTapDistanceThreshold = 12f;
    private const int DoubleTapTimeThresholdMs = 400;
    private static readonly TimeSpan DragVibrationDuration = TimeSpan.FromMilliseconds(20);

    private readonly Dictionary<long, Point> _touchStartPositions = new();
    private readonly HashSet<long> _touchMoved = new();

    private Point? _pointerPressedPosition;
    private bool _pointerMoved;
    private bool _selectionActive;
    private bool _rotationActive;
    private bool _clearSelectionOnRelease;

    private readonly Dictionary<Position, Point> _rotationStartPositions = new();
    private Point? _rotationCenter;
    private double? _rotationStartAngle;
    private Point? _lastRotationFloorPoint;

    private DateTimeOffset? _lastTapTimestamp;
    private Point? _lastTapViewPoint;
    private Position? _lastTapPosition;
    private Position? _rotationAnchorPosition;

    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ScaleAroundDancerBehavior), nameof(FloorCanvasViewModel));
        viewModel.PointerPressedCommand
            .Subscribe(command => HandlePointerPressed(viewModel, command))
            .DisposeWith(disposables);

        viewModel.PointerMovedCommand
            .Subscribe(command => HandlePointerMoved(viewModel, command))
            .DisposeWith(disposables);

        viewModel.PointerReleasedCommand
            .Subscribe(command => HandlePointerReleased(viewModel, command))
            .DisposeWith(disposables);

        viewModel.TouchCommand
            .Subscribe(command => HandleTouch(viewModel, command))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(_ => ClearSelection())
            .DisposeWith(disposables);

        globalState
            .WhenAnyValue(state => state.InteractionMode)
            .Subscribe(mode =>
            {
                if (mode != Global.InteractionMode.RotateAroundDancer
                    && mode != Global.InteractionMode.Move
                    && mode != Global.InteractionMode.RotateAroundCenter
                    && mode != Global.InteractionMode.Scale)
                {
                    ClearSelection();
                }
            })
            .DisposeWith(disposables);
    }

    private void HandlePointerPressed(FloorCanvasViewModel viewModel, PointerPressedCommand command)
    {
        if (!IsRotateModeActive())
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView as Element);
        if (position is null || command.EventArgs.Button != ButtonsMask.Primary)
        {
            ResetPointerState();
            return;
        }

        _pointerPressedPosition = position.Value;
        _pointerMoved = false;

        if (!TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            _clearSelectionOnRelease = true;
            return;
        }

        if (globalState.SelectedPositions.Count > 0 && !_selectionActive)
        {
            StartRotation(floorPoint);
            return;
        }

        StartSelection(floorPoint);
    }

    private void HandlePointerMoved(FloorCanvasViewModel viewModel, PointerMovedCommand command)
    {
        if (!IsRotateModeActive())
        {
            return;
        }

        if (_pointerPressedPosition is null || command.EventArgs.Button != ButtonsMask.Primary)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView as Element);
        if (position is null)
        {
            return;
        }

        var deltaX = position.Value.X - _pointerPressedPosition.Value.X;
        var deltaY = position.Value.Y - _pointerPressedPosition.Value.Y;
        var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        if (distance > PointerMoveThreshold)
        {
            _pointerMoved = true;
        }

        if (!TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            return;
        }

        if (_rotationActive && _pointerMoved)
        {
            UpdateRotation(floorPoint);
            return;
        }

        if (_selectionActive)
        {
            UpdateSelection(floorPoint);
        }
    }

    private void HandlePointerReleased(FloorCanvasViewModel viewModel, PointerReleasedCommand command)
    {
        if (!IsRotateModeActive())
        {
            ResetPointerState();
            return;
        }

        if (_pointerPressedPosition is null)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(viewModel.CanvasView as Element);
        if (position is not null && TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            var isTapOnPosition = false;
            var isDoubleTap = !_pointerMoved && TryHandleDoubleTap(position.Value, floorPoint, out isTapOnPosition);
            if (isDoubleTap)
            {
                CancelSelectionForDoubleTap();
                CancelRotation();
                ResetPointerState();
                redrawFloorPublisher.Publish(new RedrawFloorCommand());
                return;
            }

            if (!_pointerMoved && isTapOnPosition)
            {
                CancelSelectionForDoubleTap();
                CancelRotation();
                ResetPointerState();
                redrawFloorPublisher.Publish(new RedrawFloorCommand());
                return;
            }

            if (_rotationActive)
            {
                if (_pointerMoved)
                {
                    CompleteRotation();
                }
                else
                {
                    ClearSelection();
                }
            }
            else if (_selectionActive)
            {
                CompleteSelection();
            }
        }
        else
        {
            if (_clearSelectionOnRelease)
            {
                ClearSelection();
            }

            if (_selectionActive)
            {
                CompleteSelection();
            }
        }

        ResetPointerState();
    }

    private void HandleTouch(FloorCanvasViewModel viewModel, TouchCommand command)
    {
        if (!IsRotateModeActive())
        {
            return;
        }

        var args = command.EventArgs;
        if (args.DeviceType != SKTouchDeviceType.Touch)
        {
            return;
        }

        switch (args.ActionType)
        {
            case SKTouchAction.Pressed:
                _touchStartPositions[args.Id] = ToViewPoint(args.Location, command.CanvasView);
                _touchMoved.Remove(args.Id);
                HandleTouchPress(viewModel, args.Id);
                break;

            case SKTouchAction.Moved:
                HandleTouchMove(viewModel, args.Id, args.Location);
                break;

            case SKTouchAction.Released:
                HandleTouchRelease(viewModel, args.Id, args.Location);
                _touchStartPositions.Remove(args.Id);
                _touchMoved.Remove(args.Id);
                break;

            case SKTouchAction.Cancelled:
                _touchStartPositions.Remove(args.Id);
                _touchMoved.Remove(args.Id);
                ResetPointerState();
                break;
        }
    }

    private void HandleTouchPress(FloorCanvasViewModel viewModel, long touchId)
    {
        if (!_touchStartPositions.TryGetValue(touchId, out var viewPoint))
        {
            return;
        }

        _pointerPressedPosition = viewPoint;
        _pointerMoved = false;

        if (!TryGetFloorPoint(viewModel, viewPoint, out var floorPoint))
        {
            _clearSelectionOnRelease = true;
            return;
        }

        if (globalState.SelectedPositions.Count > 0 && !_selectionActive)
        {
            StartRotation(floorPoint);
            return;
        }

        StartSelection(floorPoint);
    }

    private void HandleTouchMove(FloorCanvasViewModel viewModel, long touchId, SKPoint location)
    {
        if (!_touchStartPositions.TryGetValue(touchId, out var startViewPoint)
            || viewModel.CanvasView is not { } canvasView)
        {
            return;
        }

        var viewPoint = ToViewPoint(location, canvasView);
        var deltaX = viewPoint.X - startViewPoint.X;
        var deltaY = viewPoint.Y - startViewPoint.Y;
        var distance = MathF.Sqrt((float)(deltaX * deltaX + deltaY * deltaY));
        if (distance > PointerMoveThreshold)
        {
            _touchMoved.Add(touchId);
            _pointerMoved = true;
        }

        if (!TryGetFloorPoint(viewModel, viewPoint, out var floorPoint))
        {
            return;
        }

        if (_rotationActive && _pointerMoved)
        {
            UpdateRotation(floorPoint);
            return;
        }

        if (_selectionActive)
        {
            UpdateSelection(floorPoint);
        }
    }

    private void HandleTouchRelease(FloorCanvasViewModel viewModel, long touchId, SKPoint location)
    {
        if (viewModel.CanvasView is not { } canvasView)
        {
            return;
        }

        var viewPoint = ToViewPoint(location, canvasView);
        if (!TryGetFloorPoint(viewModel, viewPoint, out var floorPoint))
        {
            if (_clearSelectionOnRelease)
            {
                ClearSelection();
            }

            CompleteSelection();
            ResetPointerState();
            return;
        }

        var isTapOnPosition = false;
        var isDoubleTap = !_pointerMoved && TryHandleDoubleTap(viewPoint, floorPoint, out isTapOnPosition);
        if (isDoubleTap)
        {
            CancelSelectionForDoubleTap();
            CancelRotation();
            ResetPointerState();
            redrawFloorPublisher.Publish(new RedrawFloorCommand());
            return;
        }

        if (!_pointerMoved && isTapOnPosition)
        {
            CancelSelectionForDoubleTap();
            CancelRotation();
            ResetPointerState();
            redrawFloorPublisher.Publish(new RedrawFloorCommand());
            return;
        }

        if (_rotationActive)
        {
            if (_pointerMoved)
            {
                CompleteRotation();
            }
            else
            {
                ClearSelection();
            }

            ResetPointerState();
            return;
        }

        if (_selectionActive)
        {
            CompleteSelection();
        }

        ResetPointerState();
    }

    private void StartSelection(Point floorPoint)
    {
        _selectionActive = true;
        _rotationActive = false;
        _clearSelectionOnRelease = false;
        _rotationAnchorPosition = null;
        ResetTapState();
        globalState.SelectedPositions.Clear();
        globalState.SelectionRectangle = new Global.SelectionRectangle(floorPoint, floorPoint);
        stateMachine.TryApply(new ScaleAroundDancerSelectionStartedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void UpdateSelection(Point floorPoint)
    {
        if (globalState.SelectionRectangle is not { } rectangle)
        {
            rectangle = new Global.SelectionRectangle(floorPoint, floorPoint);
        }

        rectangle = rectangle with { End = floorPoint };
        globalState.SelectionRectangle = rectangle;

        var positions = GetPositionsInRectangle(globalState.SelectedScene, rectangle);
        SyncSelection(positions);
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void CompleteSelection()
    {
        if (!_pointerMoved)
        {
            globalState.SelectedPositions.Clear();
        }

        globalState.SelectionRectangle = null;
        _selectionActive = false;
        stateMachine.TryApply(new ScaleAroundDancerSelectionCompletedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void CancelSelectionForDoubleTap()
    {
        if (!_selectionActive)
        {
            return;
        }

        globalState.SelectionRectangle = null;
        _selectionActive = false;
        stateMachine.TryApply(new ScaleAroundDancerSelectionCompletedTrigger());
    }

    private void StartRotation(Point floorPoint)
    {
        if (globalState.SelectedPositions.Count == 0)
        {
            return;
        }

        _rotationStartPositions.Clear();
        var selectedPositions = globalState.SelectedPositions.ToArray();
        foreach (var selected in selectedPositions)
        {
            _rotationStartPositions[selected] = new Point(selected.X, selected.Y);
        }

        _rotationCenter = _rotationAnchorPosition is null
            ? CalculateCenter(globalState.SelectedPositions)
            : new Point(_rotationAnchorPosition.X, _rotationAnchorPosition.Y);
        _rotationStartAngle = CalculateAngle(_rotationCenter.Value, floorPoint);
        _lastRotationFloorPoint = floorPoint;
        _rotationActive = true;
        _selectionActive = false;
        _clearSelectionOnRelease = false;
        globalState.SelectionRectangle = null;
        stateMachine.TryApply(new ScaleAroundDancerDragStartedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
        if (vibration.IsSupported)
        {
            vibration.Vibrate(DragVibrationDuration);
        }
    }

    private void UpdateRotation(Point floorPoint)
    {
        if (_rotationCenter is null || _rotationStartAngle is null)
        {
            return;
        }

        var angle = CalculateAngle(_rotationCenter.Value, floorPoint);
        var delta = angle - _rotationStartAngle.Value;
        var cos = Math.Cos(delta);
        var sin = Math.Sin(delta);

        foreach (var (position, startPoint) in _rotationStartPositions)
        {
            var relativeX = startPoint.X - _rotationCenter.Value.X;
            var relativeY = startPoint.Y - _rotationCenter.Value.Y;
            var rotatedX = relativeX * cos - relativeY * sin;
            var rotatedY = relativeX * sin + relativeY * cos;
            position.X = _rotationCenter.Value.X + rotatedX;
            position.Y = _rotationCenter.Value.Y + rotatedY;
        }

        _lastRotationFloorPoint = floorPoint;
        SnapSelectedPositionsToGrid();
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void CompleteRotation()
    {
        var floorPoint = _lastRotationFloorPoint;
        if (floorPoint is null)
        {
            return;
        }

        UpdateRotation(floorPoint.Value);
        SnapSelectedPositionsToGrid();
        _rotationActive = false;
        _rotationStartPositions.Clear();
        _rotationCenter = null;
        _rotationStartAngle = null;
        _lastRotationFloorPoint = null;
        stateMachine.TryApply(new ScaleAroundDancerDragCompletedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
        if (vibration.IsSupported)
        {
            vibration.Cancel();
        }
    }

    private void CancelRotation()
    {
        if (!_rotationActive)
        {
            return;
        }

        _rotationActive = false;
        _rotationStartPositions.Clear();
        _rotationCenter = null;
        _rotationStartAngle = null;
        _lastRotationFloorPoint = null;
        stateMachine.TryApply(new ScaleAroundDancerDragCompletedTrigger());
        if (vibration.IsSupported)
        {
            vibration.Cancel();
        }
    }

    private void ClearSelection()
    {
        var wasRotationActive = _rotationActive;

        globalState.SelectedPositions.Clear();
        globalState.SelectionRectangle = null;
        _selectionActive = false;
        _rotationActive = false;
        _rotationStartPositions.Clear();
        _rotationCenter = null;
        _rotationStartAngle = null;
        _lastRotationFloorPoint = null;
        _rotationAnchorPosition = null;
        if (wasRotationActive && vibration.IsSupported)
        {
            vibration.Cancel();
        }

        ResetTapState();
        ResetPointerState();
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void ResetPointerState()
    {
        _pointerPressedPosition = null;
        _pointerMoved = false;
        _clearSelectionOnRelease = false;
    }

    private void ResetTapState()
    {
        _lastTapTimestamp = null;
        _lastTapViewPoint = null;
        _lastTapPosition = null;
    }

    private bool IsRotateModeActive()
    {
        if (globalState.InteractionMode != Global.InteractionMode.RotateAroundDancer)
        {
            return false;
        }

        return stateMachine.State is ScaleAroundDancerState
            || stateMachine.State is ScaleAroundDancerSelectionStartState
            || stateMachine.State is ScaleAroundDancerSelectionEndState
            || stateMachine.State is ScaleAroundDancerDragStartState
            || stateMachine.State is ScaleAroundDancerDragEndState;
    }

    private bool TryHandleDoubleTap(Point viewPoint, Point floorPoint, out bool isTapOnPosition)
    {
        isTapOnPosition = false;

        if (!TryGetPositionAtPoint(globalState.SelectedScene, floorPoint, out var hitPosition))
        {
            ResetTapState();
            return false;
        }

        isTapOnPosition = true;
        var now = timeProvider.GetUtcNow();
        var isDoubleTap = _lastTapTimestamp is not null
            && _lastTapViewPoint is not null
            && _lastTapPosition == hitPosition
            && (now - _lastTapTimestamp.Value).TotalMilliseconds <= DoubleTapTimeThresholdMs
            && CalculateDistance(viewPoint, _lastTapViewPoint.Value) <= DoubleTapDistanceThreshold;

        _lastTapTimestamp = now;
        _lastTapViewPoint = viewPoint;
        _lastTapPosition = hitPosition;

        if (!isDoubleTap)
        {
            return false;
        }

        SetAnchorPosition(hitPosition);
        ResetTapState();
        return true;
    }

    private void SetAnchorPosition(Position position)
    {
        _rotationAnchorPosition = position;
        if (!globalState.SelectedPositions.Contains(position))
        {
            globalState.SelectedPositions.Add(position);
        }
    }

    private static Point CalculateCenter(IReadOnlyCollection<Position> positions)
    {
        double sumX = 0d;
        double sumY = 0d;
        foreach (var position in positions)
        {
            sumX += position.X;
            sumY += position.Y;
        }

        var count = positions.Count;
        if (count == 0)
        {
            return new Point(0d, 0d);
        }

        return new Point(sumX / count, sumY / count);
    }

    private static double CalculateAngle(Point center, Point point)
    {
        return Math.Atan2(point.Y - center.Y, point.X - center.X);
    }

    private static double CalculateDistance(Point center, Point point)
    {
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private bool TryGetPositionAtPoint(SceneViewModel? scene, Point floorPoint, out Position position)
    {
        position = null!;

        if (scene is null || globalState.Choreography is not { } choreography)
        {
            return false;
        }

        var size = (double)choreography.Settings.DancerSize;
        var halfSize = size / 2d;

        foreach (var candidate in scene.Positions)
        {
            if (Math.Abs(candidate.X - floorPoint.X) <= halfSize
                && Math.Abs(candidate.Y - floorPoint.Y) <= halfSize)
            {
                position = candidate;
                return true;
            }
        }

        return false;
    }

    private IReadOnlyList<Position> GetPositionsInRectangle(SceneViewModel? scene, Global.SelectionRectangle rectangle)
    {
        if (scene is null || globalState.Choreography is not { } choreography)
        {
            return [];
        }

        var minX = Math.Min(rectangle.Start.X, rectangle.End.X);
        var maxX = Math.Max(rectangle.Start.X, rectangle.End.X);
        var minY = Math.Min(rectangle.Start.Y, rectangle.End.Y);
        var maxY = Math.Max(rectangle.Start.Y, rectangle.End.Y);

        var size = (double)choreography.Settings.DancerSize;
        var halfSize = size / 2d;

        var selected = new List<Position>();
        foreach (var candidate in scene.Positions)
        {
            var candidateMinX = candidate.X - halfSize;
            var candidateMaxX = candidate.X + halfSize;
            var candidateMinY = candidate.Y - halfSize;
            var candidateMaxY = candidate.Y + halfSize;

            var intersects = candidateMaxX >= minX
                && candidateMinX <= maxX
                && candidateMaxY >= minY
                && candidateMinY <= maxY;

            if (intersects)
            {
                selected.Add(candidate);
            }
        }

        return selected;
    }

    private void SyncSelection(IReadOnlyList<Position> selectedPositions)
    {
        var selectedSet = new HashSet<Position>(selectedPositions);

        for (int i = globalState.SelectedPositions.Count - 1; i >= 0; i--)
        {
            var existing = globalState.SelectedPositions[i];
            if (!selectedSet.Contains(existing))
            {
                globalState.SelectedPositions.RemoveAt(i);
            }
        }

        foreach (var candidate in selectedPositions)
        {
            if (!globalState.SelectedPositions.Contains(candidate))
            {
                globalState.SelectedPositions.Add(candidate);
            }
        }
    }

    private void SnapSelectedPositionsToGrid()
    {
        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        if (!choreography.Settings.SnapToGrid)
        {
            return;
        }

        var resolution = choreography.Settings.Resolution;
        if (resolution <= 0)
        {
            return;
        }

        var step = 1d / resolution;
        foreach (var position in globalState.SelectedPositions)
        {
            position.X = Math.Round(position.X / step) * step;
            position.Y = Math.Round(position.Y / step) * step;
        }
    }

    private bool TryGetFloorPoint(FloorCanvasViewModel viewModel, Point viewPoint, out Point floorPoint)
    {
        floorPoint = default;

        if (viewModel.CanvasView is not { } canvasView)
        {
            return false;
        }

        if (!viewModel.HasFloorBounds)
        {
            return false;
        }

        if (globalState.Choreography is not { } choreography)
        {
            return false;
        }

        var (scaleX, scaleY) = GetCanvasScale(canvasView);
        var canvasPoint = new SKPoint((float)(viewPoint.X * scaleX), (float)(viewPoint.Y * scaleY));

        var inverse = viewModel.TransformationMatrix.Invert();
        var transformedPoint = inverse.MapPoint(canvasPoint);
        var floorBounds = viewModel.FloorBounds;
        if (!floorBounds.Contains(transformedPoint))
        {
            return false;
        }

        var floor = choreography.Floor;
        float width = floorBounds.Width;
        float height = floorBounds.Height;
        float floorWidth = (float)(floor.SizeLeft + floor.SizeRight);
        float floorHeight = (float)(floor.SizeFront + floor.SizeBack);

        if (floorWidth <= 0f || floorHeight <= 0f || width <= 0f || height <= 0f)
        {
            return false;
        }

        float scale = Math.Min(width / floorWidth, height / floorHeight);
        if (scale <= 0f || float.IsNaN(scale) || float.IsInfinity(scale))
        {
            return false;
        }

        float centerX = floorBounds.Left + width / 2f;
        float centerY = floorBounds.Top + height / 2f;

        var positionX = (transformedPoint.X - centerX) / scale;
        var positionY = (centerY - transformedPoint.Y) / scale;
        floorPoint = new Point(positionX, positionY);
        return true;
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

    private static Point ToViewPoint(SKPoint point, ISKCanvasView canvasView)
    {
        var (scaleX, scaleY) = GetCanvasScale(canvasView);
        var x = point.X / scaleX;
        var y = point.Y / scaleY;
        return new Point(x, y);
    }
}
