using ChoreoApp.Logging;
using ChoreoApp.Models;
using ChoreoApp.Settings;
using Microsoft.Extensions.Logging;

namespace ChoreoApp;

public partial class App
{
    private static readonly ILogger Logger = AppLogger.CreateLogger<App>();
    private readonly IPreferences _preferences;

    public App(IPreferences preferences)
    {
        _preferences = preferences;

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

    private void ApplyStoredTheme(Application application)
    {
        var useSystem = _preferences.Get(SettingsPreferenceKeys.UseSystemTheme, true);
        var storedTheme = _preferences.Get(SettingsPreferenceKeys.Theme, "Light");
        var appTheme = storedTheme == "Dark" ? AppTheme.Dark : AppTheme.Light;

        if (useSystem)
        {
            application.UserAppTheme = AppTheme.Unspecified;
            MaterialSchemeHelper.ApplyMaterialScheme(application, application.RequestedTheme, _preferences);
            return;
        }

        application.UserAppTheme = appTheme;
        MaterialSchemeHelper.ApplyMaterialScheme(application, appTheme, _preferences);
    }

    private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        if (Current is not { } application)
        {
            return;
        }

        Logger.LogInformation("Theme changed to {Theme}.", e.RequestedTheme);

        var useSystem = _preferences.Get(SettingsPreferenceKeys.UseSystemTheme, true);
        _preferences.Set(SettingsPreferenceKeys.Theme, e.RequestedTheme == AppTheme.Dark ? "Dark" : "Light");

        if (useSystem)
        {
            application.UserAppTheme = AppTheme.Unspecified;
        }
        else
        {
            application.UserAppTheme = e.RequestedTheme;
        }

        MaterialSchemeHelper.ApplyMaterialScheme(application, e.RequestedTheme, _preferences);
    }

    public static void UpdateMaterialScheme(IPreferences preferences)
    {
        if (Current is not { } application)
        {
            return;
        }

        MaterialSchemeHelper.UpdateMaterialScheme(application, preferences);
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
