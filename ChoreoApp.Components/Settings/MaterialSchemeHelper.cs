using ChoreoApp.Models;
using MaterialColorUtilities;
using MaterialDesignThemes.Maui;
using Platform = MaterialColorUtilities.Platform;

namespace ChoreoApp.Settings;

public static class MaterialSchemeHelper
{
    public static void UpdateMaterialScheme(Application application, IPreferences preferences)
    {
        ApplyMaterialScheme(application, application.UserAppTheme, preferences);
    }

    public static void ApplyMaterialScheme(Application application, AppTheme theme, IPreferences preferences)
    {
        if (theme == AppTheme.Unspecified)
        {
            theme = application.RequestedTheme;
        }

        var materialDictionary = application
            .Resources
            .MergedDictionaries
            .OfType<MaterialDesignColorsDictionary>()
            .FirstOrDefault();

        if (materialDictionary is null)
        {
            return;
        }

        var isDark = theme == AppTheme.Dark;
        var contrast = 0.5;
        var defaultSource = Hct.FromInt(Color.ArgbFromColor(Color.FromRgb(0x19, 0x76, 0xD2)));

        var usePrimary = preferences.Get(SettingsPreferenceKeys.UsePrimaryColor, false);
        var useSecondary = preferences.Get(SettingsPreferenceKeys.UseSecondaryColor, false);
        var useTertiary = preferences.Get(SettingsPreferenceKeys.UseTertiaryColor, false);

        if (!usePrimary)
        {
            useSecondary = false;
            useTertiary = false;
        }
        else if (!useSecondary)
        {
            useTertiary = false;
        }

        var scheme = BuildScheme(defaultSource, isDark, contrast, usePrimary, useSecondary, useTertiary, preferences);
        materialDictionary.SetScheme(scheme);
    }

    private static SchemeContent BuildScheme(
        Hct defaultSource,
        bool isDark,
        double contrast,
        bool usePrimary,
        bool useSecondary,
        bool useTertiary,
        IPreferences preferences)
    {
        var primaryColor = TryGetColor(SettingsPreferenceKeys.PrimaryColor, defaultSource, preferences);

        if (!usePrimary)
        {
            return new SchemeContent(defaultSource, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
        }

        if (useSecondary)
        {
            var secondaryColor = TryGetColor(SettingsPreferenceKeys.SecondaryColor, primaryColor, preferences);

            if (useTertiary)
            {
                var tertiaryColor = TryGetColor(SettingsPreferenceKeys.TertiaryColor, secondaryColor, preferences);
                return new SchemeContent(primaryColor, secondaryColor, tertiaryColor, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
            }

            return new SchemeContent(primaryColor, secondaryColor, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
        }

        return new SchemeContent(primaryColor, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
    }

    private static Hct TryGetColor(string preferenceKey, Hct fallback, IPreferences preferences)
    {
        var stored = preferences.Get(preferenceKey, string.Empty);
        if (!string.IsNullOrWhiteSpace(stored) && Color.TryParse(stored, out var color))
        {
            return Hct.FromInt(Color.ArgbFromColor(color));
        }

        return fallback;
    }
}
