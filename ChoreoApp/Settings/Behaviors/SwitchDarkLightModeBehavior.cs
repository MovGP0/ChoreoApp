using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ChoreoApp.Settings.Behaviors;

public sealed class SwitchDarkLightModeBehavior: IBehavior<SettingsViewModel>
{
    public void Activate(SettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.IsDarkMode)
            .Skip(1)
            .Subscribe(isDark =>
            {
                if (Application.Current is not { } application)
                {
                    return;
                }

                application.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
            })
            .DisposeWith(disposables);
    }
}
