using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Settings;

public sealed partial class SettingsViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    private CompositeDisposable Disposables { get; } = new();
    public void Dispose() => Disposables.Dispose();

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private bool _isDarkMode;

    public SettingsViewModel(IEnumerable<IBehavior<SettingsViewModel>> behaviors)
    {
        this.WhenActivated(disposables =>
        {
            var storedTheme = Preferences.Default.Get("Theme", "Light");
            IsDarkMode = storedTheme == "Dark";

            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });

        Activator.DisposeWith(Disposables);
    }
}
