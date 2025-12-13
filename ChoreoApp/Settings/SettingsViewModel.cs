namespace ChoreoApp.Settings;

public sealed class SettingsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
