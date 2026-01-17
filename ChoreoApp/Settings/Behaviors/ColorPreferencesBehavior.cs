using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Models;

namespace ChoreoApp.Settings.Behaviors;

public sealed class ColorPreferencesBehavior(IPreferences preferences) : IBehavior<SettingsViewModel>
{
    public void Activate(SettingsViewModel viewModel, CompositeDisposable disposables)
    {
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

                App.UpdateMaterialScheme(preferences);
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

                App.UpdateMaterialScheme(preferences);
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

                App.UpdateMaterialScheme(preferences);
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
                App.UpdateMaterialScheme(preferences);
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
                App.UpdateMaterialScheme(preferences);
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
                App.UpdateMaterialScheme(preferences);
            })
            .DisposeWith(disposables);
    }
}
