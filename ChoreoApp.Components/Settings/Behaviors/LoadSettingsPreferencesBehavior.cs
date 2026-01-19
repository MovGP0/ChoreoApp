using System.Reactive.Disposables;
using ChoreoApp.Models;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Settings.Behaviors;

public sealed class LoadSettingsPreferencesBehavior(IPreferences preferences, ILogger<SettingsViewModel> logger)
    : IBehavior<SettingsViewModel>
{
    public void Activate(SettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(LoadSettingsPreferencesBehavior), nameof(SettingsViewModel));
        var storedTheme = preferences.Get(SettingsPreferenceKeys.Theme, "Light");
        viewModel.IsDarkMode = storedTheme == "Dark";

        viewModel.UseSystemTheme = preferences.Get(SettingsPreferenceKeys.UseSystemTheme, true);
        viewModel.UsePrimaryColor = preferences.Get(SettingsPreferenceKeys.UsePrimaryColor, false);
        viewModel.UseSecondaryColor = preferences.Get(SettingsPreferenceKeys.UseSecondaryColor, false)
            && viewModel.UsePrimaryColor;
        viewModel.UseTertiaryColor = preferences.Get(SettingsPreferenceKeys.UseTertiaryColor, false)
            && viewModel.UseSecondaryColor;

        viewModel.PrimaryColor = GetColorFromPreferences(
            SettingsPreferenceKeys.PrimaryColor,
            SettingsViewModel.DefaultPrimaryColor);
        viewModel.SecondaryColor = GetColorFromPreferences(
            SettingsPreferenceKeys.SecondaryColor,
            SettingsViewModel.DefaultSecondaryColor);
        viewModel.TertiaryColor = GetColorFromPreferences(
            SettingsPreferenceKeys.TertiaryColor,
            SettingsViewModel.DefaultTertiaryColor);
    }

    private Color GetColorFromPreferences(string key, Color fallback)
    {
        var stored = preferences.Get(key, string.Empty);
        if (!string.IsNullOrWhiteSpace(stored) && Color.TryParse(stored, out var parsed))
        {
            return parsed;
        }

        return fallback;
    }
}
