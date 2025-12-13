namespace ChoreoApp.Settings;

public sealed partial class SettingsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private bool _isDarkMode;

    public SettingsViewModel(IEnumerable<IBehavior<SettingsViewModel>> behaviors)
    {
        IsDarkMode = Application.Current?.UserAppTheme == AppTheme.Dark;

        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }
}
