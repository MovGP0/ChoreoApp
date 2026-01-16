namespace MaterialDesignDemo.Maui.ThemeSettings;

public sealed partial class ThemeSettingsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public IReadOnlyList<string> ContrastValues { get; } =
    [
        "Low",
        "Medium",
        "High"
    ];

    public IReadOnlyList<string> ColorSelectionValues { get; } =
    [
        "System",
        "Custom",
        "Inherit"
    ];

    [Reactive]
    private bool _isDarkTheme;

    [Reactive]
    private bool _isColorAdjusted;

    [Reactive]
    private double _desiredContrastRatio = 7;

    [Reactive]
    private string _contrastValue = "Medium";

    [Reactive]
    private string _colorSelectionValue = "System";
}
