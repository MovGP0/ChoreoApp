using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor;

public sealed class FloorCanvasViewModel : ReactiveObject, IActivatableViewModel
{
    private const float MaxZoomFactor = 5f;
    private const float MinZoomFactor = 0.2f;
    private const float PanMargin = 20f;

    private Point? _lastHoverPosition;
    private Point? _lastPanPosition;
    private Point? _lastPointerPosition;
    private float _lastPinchScale = 1f;

    private bool _hasFloorBounds;
    private SKRect _floorBounds;
    private SKSize _canvasSize;

    public FloorCanvasViewModel(
        IPublisher<DrawFloorCommand> drawFloorCommandPublisher,
        GlobalStateModel globalState,
        IEnumerable<IBehavior<FloorCanvasViewModel>> behaviors)
    {
        GlobalState = globalState;
        DrawFloorCommandPublisher = drawFloorCommandPublisher;

        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    public ViewModelActivator Activator { get; } = new();

    public GlobalStateModel GlobalState { get; }
    public IPublisher<DrawFloorCommand> DrawFloorCommandPublisher { get; }
    public SKCanvasView? CanvasView { get; set; }

    public SKMatrix TransformationMatrix { get; private set; } = SKMatrix.CreateIdentity();

    public void UpdateFloorBounds(SKRect floorBounds, SKSize canvasSize)
    {
        _floorBounds = floorBounds;
        _canvasSize = canvasSize;
        _hasFloorBounds = true;
    }

    public void HandlePanUpdated(SKCanvasView canvasView, PanUpdatedEventArgs args)
    {
        switch (args.StatusType)
        {
            case GestureStatus.Started:
                _lastPanPosition = new Point(args.TotalX, args.TotalY);
                break;
            case GestureStatus.Running:
            {
                if (_lastPanPosition is null)
                {
                    _lastPanPosition = new Point(args.TotalX, args.TotalY);
                    break;
                }

                var currentPosition = new Point(args.TotalX, args.TotalY);
                var deltaX = currentPosition.X - _lastPanPosition.Value.X;
                var deltaY = currentPosition.Y - _lastPanPosition.Value.Y;

                ApplyTranslation(canvasView, deltaX, deltaY);
                _lastPanPosition = currentPosition;
                InvalidateCanvas();
                break;
            }
            case GestureStatus.Canceled:
            case GestureStatus.Completed:
                _lastPanPosition = null;
                break;
        }
    }

    public void HandlePinchUpdated(SKCanvasView canvasView, PinchGestureUpdatedEventArgs args)
    {
        switch (args.Status)
        {
            case GestureStatus.Started:
                _lastPinchScale = 1f;
                break;
            case GestureStatus.Running:
            {
                var scale = (float)(args.Scale / _lastPinchScale);
                _lastPinchScale = (float)args.Scale;

                var (dpiScaleX, dpiScaleY) = GetCanvasScale(canvasView);
                var originX = (float)(args.ScaleOrigin.X * canvasView.Width * dpiScaleX);
                var originY = (float)(args.ScaleOrigin.Y * canvasView.Height * dpiScaleY);

                var scaleMatrix = SKMatrix.CreateScale(scale, scale, originX, originY);

                ApplyTransformation(scaleMatrix);
                InvalidateCanvas();
                break;
            }
            case GestureStatus.Canceled:
            case GestureStatus.Completed:
                _lastPinchScale = 1f;
                break;
        }
    }

    public void HandlePointerPressed(SKCanvasView canvasView, PointerEventArgs args)
    {
        var position = args.GetPosition(canvasView);
        _lastHoverPosition = position;

        if (args.Button != ButtonsMask.Primary || position is null)
        {
            _lastPointerPosition = null;
            return;
        }

        _lastPointerPosition = position.Value;
    }

    public void HandlePointerMoved(SKCanvasView canvasView, PointerEventArgs args)
    {
        var position = args.GetPosition(canvasView);
        if (position is null)
        {
            return;
        }

        _lastHoverPosition = position.Value;

        if (_lastPointerPosition is null || args.Button != ButtonsMask.Primary)
        {
            return;
        }

        var deltaX = position.Value.X - _lastPointerPosition.Value.X;
        var deltaY = position.Value.Y - _lastPointerPosition.Value.Y;

        ApplyTranslation(canvasView, deltaX, deltaY);
        _lastPointerPosition = position.Value;
        InvalidateCanvas();
    }

    public void HandlePointerReleased(PointerEventArgs args)
    {
        _lastPointerPosition = null;
    }

    public void HandlePointerWheelChanged(SKCanvasView canvasView, double delta, Point? position)
    {
        var zoomFactor = delta > 0 ? 1.1f : 0.9f;
        var zoomCenter = position ?? _lastHoverPosition;
        if (zoomCenter is null)
        {
            return;
        }

        var (dpiScaleX, dpiScaleY) = GetCanvasScale(canvasView);
        var originX = (float)(zoomCenter.Value.X * dpiScaleX);
        var originY = (float)(zoomCenter.Value.Y * dpiScaleY);

        var scaleMatrix = SKMatrix.CreateScale(zoomFactor, zoomFactor, originX, originY);
        ApplyTransformation(scaleMatrix);
        InvalidateCanvas();
    }

    private void ApplyTranslation(SKCanvasView canvasView, double deltaX, double deltaY)
    {
        var (dpiScaleX, dpiScaleY) = GetCanvasScale(canvasView);
        var sx = TransformationMatrix.ScaleX;
        var sy = TransformationMatrix.ScaleY;

        var translationMatrix = SKMatrix.CreateTranslation(
            (float)(deltaX * dpiScaleX) / sx,
            (float)(deltaY * dpiScaleY) / sy);

        ApplyTransformation(translationMatrix);
    }

    private void InvalidateCanvas()
    {
        CanvasView?.InvalidateSurface();
    }

    private void ApplyTransformation(SKMatrix newMatrix)
    {
        var newTransformationMatrix = SKMatrix.Concat(TransformationMatrix, newMatrix);
        var scaleX = newTransformationMatrix.ScaleX;
        var scaleY = newTransformationMatrix.ScaleY;

        if (scaleX <= MaxZoomFactor
            && scaleY <= MaxZoomFactor
            && scaleX >= MinZoomFactor
            && scaleY >= MinZoomFactor)
        {
            TransformationMatrix = ClampTranslation(newTransformationMatrix);
        }
    }

    private SKMatrix ClampTranslation(SKMatrix matrix)
    {
        if (!_hasFloorBounds || _canvasSize.Width <= 0 || _canvasSize.Height <= 0)
        {
            return matrix;
        }

        var scaleX = matrix.ScaleX;
        var scaleY = matrix.ScaleY;

        var marginX = PanMargin * scaleX;
        var marginY = PanMargin * scaleY;

        var minTransX = -_floorBounds.Right * scaleX + marginX;
        var maxTransX = _canvasSize.Width - _floorBounds.Left * scaleX - marginX;
        var minTransY = -_floorBounds.Bottom * scaleY + marginY;
        var maxTransY = _canvasSize.Height - _floorBounds.Top * scaleY - marginY;

        var clampedTransX = Math.Clamp(matrix.TransX, minTransX, maxTransX);
        var clampedTransY = Math.Clamp(matrix.TransY, minTransY, maxTransY);

        matrix.TransX = clampedTransX;
        matrix.TransY = clampedTransY;
        return matrix;
    }

    private static (float ScaleX, float ScaleY) GetCanvasScale(SKCanvasView canvasView)
    {
        var width = canvasView.Width;
        var height = canvasView.Height;

        if (width <= 0 || height <= 0)
        {
            return (1f, 1f);
        }

        var scaleX = canvasView.CanvasSize.Width / (float)width;
        var scaleY = canvasView.CanvasSize.Height / (float)height;
        return (scaleX, scaleY);
    }
}
