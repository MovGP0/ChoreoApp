namespace MaterialDesignDemo.Maui.ColorZones;

public sealed partial class ColorZonesViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
