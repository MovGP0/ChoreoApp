using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Settings;

public sealed partial class SettingsViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    private static readonly Color DefaultPrimaryColor = Color.FromRgb(0x19, 0x76, 0xD2);
    private static readonly Color DefaultSecondaryColor = Color.FromRgb(0x67, 0x5A, 0x84);
    private static readonly Color DefaultTertiaryColor = Color.FromRgb(0x82, 0x5A, 0x2C);

    private CompositeDisposable Disposables { get; } = new();
    public void Dispose() => Disposables.Dispose();

    public ViewModelActivator Activator { get; } = new();
    public IReadOnlyList<Models.MaterialColorGroup> ColorGroups { get; } = Models.MaterialColorPalette.BuildGroups();

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

    public SettingsViewModel(IEnumerable<IBehavior<SettingsViewModel>> behaviors)
    {
        this.WhenActivated(disposables =>
        {
            var storedTheme = Preferences.Default.Get(SettingsPreferenceKeys.Theme, "Light");
            IsDarkMode = storedTheme == "Dark";

            UseSystemTheme = Preferences.Default.Get(SettingsPreferenceKeys.UseSystemTheme, true);
            UsePrimaryColor = Preferences.Default.Get(SettingsPreferenceKeys.UsePrimaryColor, false);
            UseSecondaryColor = Preferences.Default.Get(SettingsPreferenceKeys.UseSecondaryColor, false) && UsePrimaryColor;
            UseTertiaryColor = Preferences.Default.Get(SettingsPreferenceKeys.UseTertiaryColor, false) && UseSecondaryColor;

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

    private static Color GetColorFromPreferences(string key, Color fallback)
    {
        var stored = Preferences.Default.Get(key, string.Empty);
        if (!string.IsNullOrWhiteSpace(stored) && Color.TryParse(stored, out var parsed))
        {
            return parsed;
        }

        return fallback;
    }
}
