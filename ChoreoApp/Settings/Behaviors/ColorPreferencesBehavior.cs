using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Models;

namespace ChoreoApp.Settings.Behaviors;

public sealed class ColorPreferencesBehavior : IBehavior<SettingsViewModel>
{
    public void Activate(SettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.UsePrimaryColor)
            .Skip(1)
            .Subscribe(enabled =>
            {
                Preferences.Default.Set(SettingsPreferenceKeys.UsePrimaryColor, enabled);

                if (!enabled)
                {
                    Preferences.Default.Remove(SettingsPreferenceKeys.PrimaryColor);
                    viewModel.UseSecondaryColor = false;
                    viewModel.UseTertiaryColor = false;
                }

                App.UpdateMaterialScheme();
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

                Preferences.Default.Set(SettingsPreferenceKeys.UseSecondaryColor, enabled);

                if (!enabled)
                {
                    Preferences.Default.Remove(SettingsPreferenceKeys.SecondaryColor);
                    viewModel.UseTertiaryColor = false;
                }

                App.UpdateMaterialScheme();
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

                Preferences.Default.Set(SettingsPreferenceKeys.UseTertiaryColor, enabled);

                if (!enabled)
                {
                    Preferences.Default.Remove(SettingsPreferenceKeys.TertiaryColor);
                }

                App.UpdateMaterialScheme();
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

                Preferences.Default.Set(SettingsPreferenceKeys.PrimaryColor, color.ToArgbHex());
                App.UpdateMaterialScheme();
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

                Preferences.Default.Set(SettingsPreferenceKeys.SecondaryColor, color.ToArgbHex());
                App.UpdateMaterialScheme();
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

                Preferences.Default.Set(SettingsPreferenceKeys.TertiaryColor, color.ToArgbHex());
                App.UpdateMaterialScheme();
            })
            .DisposeWith(disposables);
    }
}
