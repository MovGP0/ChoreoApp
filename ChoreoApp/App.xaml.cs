using ChoreoApp.Styling;
using MaterialColorUtilities;
using Platform = MaterialColorUtilities.Platform;

namespace ChoreoApp;

public partial class App
{
    public App()
    {
        InitializeComponent();

        if (Current is { } application)
        {
            application.RequestedThemeChanged += OnRequestedThemeChanged;
            ApplyStoredTheme(application);
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    private static void ApplyStoredTheme(Application application)
    {
        var storedTheme = Preferences.Default.Get("Theme", "Light");
        var appTheme = storedTheme == "Dark" ? AppTheme.Dark : AppTheme.Light;

        application.UserAppTheme = appTheme;
        SetMaterialScheme(appTheme);
    }

    private static void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        if (Current is not { } application)
        {
            return;
        }

        Preferences.Default.Set("Theme", e.RequestedTheme == AppTheme.Dark ? "Dark" : "Light");
        application.UserAppTheme = e.RequestedTheme;
        SetMaterialScheme(e.RequestedTheme);
    }

    private static void SetMaterialScheme(AppTheme theme)
    {
        if (Current is not { } application)
        {
            return;
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

        var scheme = new SchemeContent(
            Hct.FromInt(Color.ArgbFromColor(Color.FromRgb(0x19, 0x76, 0xD2))),
            theme == AppTheme.Dark,
            0.5,
            SpecVersion.Spec2025,
            Platform.Phone);

        materialDictionary.SetScheme(scheme);
    }
}

