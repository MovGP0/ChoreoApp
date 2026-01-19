using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Models;

namespace ChoreoApp.Settings;

public sealed partial class SettingsViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    internal static readonly Color DefaultPrimaryColor = Color.FromRgb(0x19, 0x76, 0xD2);
    internal static readonly Color DefaultSecondaryColor = Color.FromRgb(0x67, 0x5A, 0x84);
    internal static readonly Color DefaultTertiaryColor = Color.FromRgb(0x82, 0x5A, 0x2C);

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

    public SettingsViewModel(IEnumerable<IBehavior<SettingsViewModel>> behaviors)
    {
        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });

        Activator.DisposeWith(Disposables);
    }
}
