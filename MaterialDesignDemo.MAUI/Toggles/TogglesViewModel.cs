namespace MaterialDesignDemo.Maui.Toggles;

public sealed class TogglesViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
