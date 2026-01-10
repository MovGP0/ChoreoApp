using ChoreoApp.Logging;
using ChoreoApp.Models;
using ChoreoApp.Settings;
using Microsoft.Extensions.Logging;
using MaterialDesignThemes.Maui;
using MaterialColorUtilities;
using Platform = MaterialColorUtilities.Platform;

namespace ChoreoApp;

public partial class App
{
    private static readonly ILogger Logger = AppLogger.CreateLogger<App>();

    public App()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Logger.LogAppInitializationError(ex);
            throw;
        }

        if (Current is { } application)
        {
            application.RequestedThemeChanged += OnRequestedThemeChanged;
            ApplyStoredTheme(application);
        }

        HookUnhandledExceptions();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    private static void ApplyStoredTheme(Application application)
    {
        var useSystem = Preferences.Default.Get(SettingsPreferenceKeys.UseSystemTheme, true);
        var storedTheme = Preferences.Default.Get(SettingsPreferenceKeys.Theme, "Light");
        var appTheme = storedTheme == "Dark" ? AppTheme.Dark : AppTheme.Light;

        if (useSystem)
        {
            application.UserAppTheme = AppTheme.Unspecified;
            SetMaterialScheme(application.RequestedTheme);
            return;
        }

        application.UserAppTheme = appTheme;
        SetMaterialScheme(appTheme);
    }

    private static void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        if (Current is not { } application)
        {
            return;
        }

        Logger.LogInformation("Theme changed to {Theme}.", e.RequestedTheme);

        var useSystem = Preferences.Default.Get(SettingsPreferenceKeys.UseSystemTheme, true);
        Preferences.Default.Set(SettingsPreferenceKeys.Theme, e.RequestedTheme == AppTheme.Dark ? "Dark" : "Light");

        if (useSystem)
        {
            application.UserAppTheme = AppTheme.Unspecified;
        }
        else
        {
            application.UserAppTheme = e.RequestedTheme;
        }

        SetMaterialScheme(e.RequestedTheme);
    }

    private static void SetMaterialScheme(AppTheme theme)
    {
        if (Current is not { } application)
        {
            return;
        }

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

        var usePrimary = Preferences.Default.Get(SettingsPreferenceKeys.UsePrimaryColor, false);
        var useSecondary = Preferences.Default.Get(SettingsPreferenceKeys.UseSecondaryColor, false);
        var useTertiary = Preferences.Default.Get(SettingsPreferenceKeys.UseTertiaryColor, false);

        if (!usePrimary)
        {
            useSecondary = false;
            useTertiary = false;
        }
        else if (!useSecondary)
        {
            useTertiary = false;
        }

        var scheme = BuildScheme(defaultSource, isDark, contrast, usePrimary, useSecondary, useTertiary);

        materialDictionary.SetScheme(scheme);
    }

    public static void UpdateMaterialScheme()
    {
        if (Current is not { } application)
        {
            return;
        }

        SetMaterialScheme(application.UserAppTheme);
    }

    private static SchemeContent BuildScheme(
        Hct defaultSource,
        bool isDark,
        double contrast,
        bool usePrimary,
        bool useSecondary,
        bool useTertiary)
    {
        var primaryColor = TryGetColor(SettingsPreferenceKeys.PrimaryColor, defaultSource);

        if (!usePrimary)
        {
            return new SchemeContent(defaultSource, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
        }

        if (useSecondary)
        {
            var secondaryColor = TryGetColor(SettingsPreferenceKeys.SecondaryColor, primaryColor);

            if (useTertiary)
            {
                var tertiaryColor = TryGetColor(SettingsPreferenceKeys.TertiaryColor, secondaryColor);
                return new SchemeContent(primaryColor, secondaryColor, tertiaryColor, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
            }

            return new SchemeContent(primaryColor, secondaryColor, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
        }

        return new SchemeContent(primaryColor, isDark, contrast, SpecVersion.Spec2025, Platform.Phone);
    }

    private static Hct TryGetColor(string preferenceKey, Hct fallback)
    {
        var stored = Preferences.Default.Get(preferenceKey, string.Empty);
        if (!string.IsNullOrWhiteSpace(stored) && Color.TryParse(stored, out var color))
        {
            return Hct.FromInt(Color.ArgbFromColor(color));
        }

        return fallback;
    }

    private void HookUnhandledExceptions()
    {
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;
    }

    private static void OnAppDomainUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            Logger.LogUnhandledException(exception, e.IsTerminating);
        }
        else
        {
            Logger.LogUnhandledException(e.IsTerminating);
        }
    }

    private static void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Logger.LogUnobservedTaskException(e.Exception);
        e.SetObserved();
    }
}
