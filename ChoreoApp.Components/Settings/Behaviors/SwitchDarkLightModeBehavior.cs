using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Models;

namespace ChoreoApp.Settings.Behaviors;

public sealed class SwitchDarkLightModeBehavior(IPreferences preferences) : IBehavior<SettingsViewModel>
{
    public void Activate(SettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.IsDarkMode)
            .Skip(1)
            .Subscribe(isDark =>
            {
                if (viewModel.UseSystemTheme)
                {
                    return;
                }

                if (Application.Current is not { } application)
                {
                    return;
                }

                var theme = isDark ? "Dark" : "Light";
                preferences.Set(SettingsPreferenceKeys.Theme, theme);
                application.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
                MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.UseSystemTheme)
            .Skip(1)
            .Subscribe(useSystem =>
            {
                preferences.Set(SettingsPreferenceKeys.UseSystemTheme, useSystem);

                if (Application.Current is not { } application)
                {
                    return;
                }

                if (useSystem)
                {
                    application.UserAppTheme = AppTheme.Unspecified;
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                    return;
                }

                var isDark = viewModel.IsDarkMode;
                var theme = isDark ? "Dark" : "Light";
                preferences.Set(SettingsPreferenceKeys.Theme, theme);
                application.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
                MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
            })
            .DisposeWith(disposables);
    }
}
