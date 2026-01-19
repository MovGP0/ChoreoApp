using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Models;
using ChoreoApp.Settings;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Settings.Behaviors;

public sealed class ColorPreferencesBehavior(IPreferences preferences, ILogger<SettingsViewModel> logger) : IBehavior<SettingsViewModel>
{
    public void Activate(SettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ColorPreferencesBehavior), nameof(SettingsViewModel));
        viewModel
            .WhenAnyValue(vm => vm.UsePrimaryColor)
            .Skip(1)
            .Subscribe(enabled =>
            {
                preferences.Set(SettingsPreferenceKeys.UsePrimaryColor, enabled);

                if (!enabled)
                {
                    preferences.Remove(SettingsPreferenceKeys.PrimaryColor);
                    viewModel.UseSecondaryColor = false;
                    viewModel.UseTertiaryColor = false;
                }

                if (Application.Current is { } application)
                {
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.UseSecondaryColor)
            .Skip(1)
            .Subscribe(enabled =>
            {
                if (enabled && !viewModel.UsePrimaryColor)
                {
                    viewModel.UseSecondaryColor = false;
                    return;
                }

                preferences.Set(SettingsPreferenceKeys.UseSecondaryColor, enabled);

                if (!enabled)
                {
                    preferences.Remove(SettingsPreferenceKeys.SecondaryColor);
                    viewModel.UseTertiaryColor = false;
                }

                if (Application.Current is { } application)
                {
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.UseTertiaryColor)
            .Skip(1)
            .Subscribe(enabled =>
            {
                if (enabled && !viewModel.UseSecondaryColor)
                {
                    viewModel.UseTertiaryColor = false;
                    return;
                }

                preferences.Set(SettingsPreferenceKeys.UseTertiaryColor, enabled);

                if (!enabled)
                {
                    preferences.Remove(SettingsPreferenceKeys.TertiaryColor);
                }

                if (Application.Current is { } application)
                {
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.PrimaryColor)
            .Skip(1)
            .Subscribe(color =>
            {
                if (!viewModel.UsePrimaryColor)
                {
                    return;
                }

                preferences.Set(SettingsPreferenceKeys.PrimaryColor, color.ToArgbHex());
                if (Application.Current is { } application)
                {
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SecondaryColor)
            .Skip(1)
            .Subscribe(color =>
            {
                if (!viewModel.UseSecondaryColor)
                {
                    return;
                }

                preferences.Set(SettingsPreferenceKeys.SecondaryColor, color.ToArgbHex());
                if (Application.Current is { } application)
                {
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.TertiaryColor)
            .Skip(1)
            .Subscribe(color =>
            {
                if (!viewModel.UseTertiaryColor)
                {
                    return;
                }

                preferences.Set(SettingsPreferenceKeys.TertiaryColor, color.ToArgbHex());
                if (Application.Current is { } application)
                {
                    MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
                }
            })
            .DisposeWith(disposables);
    }
}
