using ChoreoApp.Floor.Messages;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor;

public sealed partial class FloorCanvasViewModel : ReactiveObject, IActivatableViewModel
{
    internal const float MaxZoomFactor = 5f;
    internal const float MinZoomFactor = 0.2f;
    internal const float PanMargin = 20f;

    private bool _hasFloorBounds;
    private SKRect _floorBounds;
    private SKSize _canvasSize;

    public FloorCanvasViewModel(
        IPublisher<DrawFloorCommand> drawFloorCommandPublisher,
        IEnumerable<IBehavior<FloorCanvasViewModel>> behaviors)
    {
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
    public IPublisher<DrawFloorCommand> DrawFloorCommandPublisher { get; }
    public ISKCanvasView? CanvasView { get; set; }

    public SKMatrix TransformationMatrix { get; internal set; } = SKMatrix.CreateIdentity();

    internal bool HasFloorBounds => _hasFloorBounds;
    internal SKRect FloorBounds => _floorBounds;
    internal SKSize CanvasSize => _canvasSize;

    public void UpdateFloorBounds(SKRect floorBounds, SKSize canvasSize)
    {
        _floorBounds = floorBounds;
        _canvasSize = canvasSize;
        _hasFloorBounds = true;
    }

    [ReactiveCommand]
    private Task<PanUpdatedCommand> PanUpdatedAsync(PanUpdatedCommand command) => Task.FromResult(command);

    [ReactiveCommand]
    private Task<PinchUpdatedCommand> PinchUpdatedAsync(PinchUpdatedCommand command) => Task.FromResult(command);

    [ReactiveCommand]
    private Task<PointerPressedCommand> PointerPressedAsync(PointerPressedCommand command) => Task.FromResult(command);

    [ReactiveCommand]
    private Task<PointerMovedCommand> PointerMovedAsync(PointerMovedCommand command) => Task.FromResult(command);

    [ReactiveCommand]
    private Task<PointerReleasedCommand> PointerReleasedAsync(PointerReleasedCommand command) => Task.FromResult(command);

    [ReactiveCommand]
    private Task<PointerWheelChangedCommand> PointerWheelChangedAsync(PointerWheelChangedCommand command) => Task.FromResult(command);

    [ReactiveCommand]
    private Task<TouchCommand> TouchAsync(TouchCommand command) => Task.FromResult(command);
}
