namespace ChoreoApp.Floor;

public sealed class FloorCanvasViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
