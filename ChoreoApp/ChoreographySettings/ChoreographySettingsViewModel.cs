using System.Globalization;
using ChoreoApp.ColorPicker;

namespace ChoreoApp.ChoreographySettings;

public sealed partial class ChoreographySettingsViewModel : ReactiveObject, IActivatableViewModel
{
    public ChoreographySettingsViewModel(IEnumerable<IBehavior<ChoreographySettingsViewModel>> behaviors)
    {
        FloorSizeOptions = Enumerable.Range(0, 101).ToList();
        GridSizeOptions = BuildGridSizeOptions();
        SelectedGridSizeOption = GridSizeOptions[0];

        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    public ViewModelActivator Activator { get; } = new();

    public IReadOnlyList<int> FloorSizeOptions { get; }
    public IReadOnlyList<GridSizeOption> GridSizeOptions { get; }
    public IReadOnlyList<MaterialColorGroup> ColorGroups { get; } = MaterialColorPalette.BuildGroups();

    public int GridResolution
    {
        get => SelectedGridSizeOption?.Value ?? GridSizeOptions[0].Value;
        set => SelectedGridSizeOption = GridSizeOptions.FirstOrDefault(option => option.Value == value)
                                        ?? GridSizeOptions[0];
    }

    [Reactive]
    private int _floorFront;

    [Reactive]
    private int _floorBack;

    [Reactive]
    private int _floorLeft;

    [Reactive]
    private int _floorRight;

    [Reactive]
    private GridSizeOption? _selectedGridSizeOption;

    [Reactive]
    private bool _drawPathFrom = true;

    [Reactive]
    private bool _drawPathTo = true;

    [Reactive]
    private bool _gridLines = true;

    [Reactive]
    private Color _floorColor = Colors.Transparent;

    [Reactive]
    private bool _showTimestamps;

    [Reactive]
    private bool _positionsAtSide = true;

    [Reactive]
    private decimal _transparency;

    [Reactive]
    private string _comment = string.Empty;

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private string _subtitle = string.Empty;

    [Reactive]
    private DateTime _date = DateTime.Today;

    [Reactive]
    private string _variation = string.Empty;

    [Reactive]
    private string _author = string.Empty;

    [Reactive]
    private string _description = string.Empty;

    private static IReadOnlyList<GridSizeOption> BuildGridSizeOptions()
    {
        var options = new List<GridSizeOption>();
        for (int denominator = 1; denominator <= 16; denominator++)
        {
            decimal centimeters = 100m / denominator;
            var centimetersText = centimeters.ToString("0.##", CultureInfo.CurrentUICulture);
            var display = $"1/{denominator} m ({centimetersText} cm)";
            options.Add(new GridSizeOption(denominator, display));
        }

        return options;
    }
}
