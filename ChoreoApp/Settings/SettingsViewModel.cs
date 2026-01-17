using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Models;

namespace ChoreoApp.Settings;

public sealed partial class SettingsViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    private static readonly Color DefaultPrimaryColor = Color.FromRgb(0x19, 0x76, 0xD2);
    private static readonly Color DefaultSecondaryColor = Color.FromRgb(0x67, 0x5A, 0x84);
    private static readonly Color DefaultTertiaryColor = Color.FromRgb(0x82, 0x5A, 0x2C);
    private readonly IPreferences _preferences;

    private CompositeDisposable Disposables { get; } = new();
    public void Dispose() => Disposables.Dispose();

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private bool _isDarkMode;

    [Reactive]
    private bool _useSystemTheme;

    [Reactive]
    private bool _usePrimaryColor;

    [Reactive]
    private bool _useSecondaryColor;

    [Reactive]
    private bool _useTertiaryColor;

    [Reactive]
    private Color _primaryColor = DefaultPrimaryColor;

    [Reactive]
    private Color _secondaryColor = DefaultSecondaryColor;

    [Reactive]
    private Color _tertiaryColor = DefaultTertiaryColor;

    public SettingsViewModel(IEnumerable<IBehavior<SettingsViewModel>> behaviors, IPreferences preferences)
    {
        _preferences = preferences;

        this.WhenActivated(disposables =>
        {
            var storedTheme = _preferences.Get(SettingsPreferenceKeys.Theme, "Light");
            IsDarkMode = storedTheme == "Dark";

            UseSystemTheme = _preferences.Get(SettingsPreferenceKeys.UseSystemTheme, true);
            UsePrimaryColor = _preferences.Get(SettingsPreferenceKeys.UsePrimaryColor, false);
            UseSecondaryColor = _preferences.Get(SettingsPreferenceKeys.UseSecondaryColor, false) && UsePrimaryColor;
            UseTertiaryColor = _preferences.Get(SettingsPreferenceKeys.UseTertiaryColor, false) && UseSecondaryColor;

            PrimaryColor = GetColorFromPreferences(SettingsPreferenceKeys.PrimaryColor, DefaultPrimaryColor);
            SecondaryColor = GetColorFromPreferences(SettingsPreferenceKeys.SecondaryColor, DefaultSecondaryColor);
            TertiaryColor = GetColorFromPreferences(SettingsPreferenceKeys.TertiaryColor, DefaultTertiaryColor);

            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });

        Activator.DisposeWith(Disposables);
    }

    private Color GetColorFromPreferences(string key, Color fallback)
    {
        var stored = _preferences.Get(key, string.Empty);
        if (!string.IsNullOrWhiteSpace(stored) && Color.TryParse(stored, out var parsed))
        {
            return parsed;
        }

        return fallback;
    }
}
