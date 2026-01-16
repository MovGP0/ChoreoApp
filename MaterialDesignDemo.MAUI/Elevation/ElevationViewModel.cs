namespace MaterialDesignDemo.Maui.Elevation;

public sealed partial class ElevationViewModel : ReactiveObject, IActivatableViewModel
{
    public ElevationViewModel()
    {
    }

    public ViewModelActivator Activator { get; } = new();
}
