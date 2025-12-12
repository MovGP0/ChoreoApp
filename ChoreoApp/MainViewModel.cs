namespace ChoreoApp;

public sealed partial class MainViewModel: ReactiveObject, IActivatableViewModel
{
    [Reactive]
    private int _count;

    public ViewModelActivator Activator { get; } = new();
}
