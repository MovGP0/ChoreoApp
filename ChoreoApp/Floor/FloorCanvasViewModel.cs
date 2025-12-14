using MessagePipe;
using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor;

public sealed class FloorCanvasViewModel : ReactiveObject, IActivatableViewModel
{
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
}
