using System.Globalization;
using ChoreoApp.ColorPicker;

namespace ChoreoApp.ChoreographySettings;

public sealed partial class ChoreographySettingsViewModel : ReactiveObject, IActivatableViewModel
{
    private static readonly TimeSpan MaxSceneTimestamp = TimeSpan.FromMinutes(1440);
    private bool _isUpdatingSceneTimestamp;

    public ChoreographySettingsViewModel(IEnumerable<IBehavior<ChoreographySettingsViewModel>> behaviors)
    {
        FloorSizeOptions = Enumerable.Range(0, 101).ToList();
        GridSizeOptions = BuildGridSizeOptions();
        SelectedGridSizeOption = GridSizeOptions[0];

        SetupSceneTimestampSynchronization();

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

    [Reactive]
    private bool _hasSelectedScene;

    [Reactive]
    private string _sceneName = string.Empty;

    [Reactive]
    private string _sceneText = string.Empty;

    [Reactive]
    private bool _sceneFixedPositions;

    [Reactive]
    private bool _sceneHasTimestamp;

    [Reactive]
    private TimeSpan _sceneTimestamp;

    [Reactive]
    private int _sceneTimestampMinutes;

    [Reactive]
    private int _sceneTimestampSeconds;

    [Reactive]
    private int _sceneTimestampMilliseconds;

    [Reactive]
    private Color _sceneColor = Colors.Transparent;

    private void SetupSceneTimestampSynchronization()
    {
        this.WhenAnyValue(vm => vm.SceneTimestamp)
            .Subscribe(sceneTimestamp =>
            {
                if (_isUpdatingSceneTimestamp)
                {
                    return;
                }

                _isUpdatingSceneTimestamp = true;
                var clamped = ClampSceneTimestamp(sceneTimestamp);
                SceneTimestampMinutes = (int)clamped.TotalMinutes;
                SceneTimestampSeconds = clamped.Seconds;
                SceneTimestampMilliseconds = (clamped.Milliseconds / 10) * 10;
                _isUpdatingSceneTimestamp = false;
            });

        this.WhenAnyValue(vm => vm.SceneTimestampMinutes, vm => vm.SceneTimestampSeconds, vm => vm.SceneTimestampMilliseconds)
            .Subscribe(tuple =>
            {
                if (_isUpdatingSceneTimestamp)
                {
                    return;
                }

                _isUpdatingSceneTimestamp = true;
                var minutes = Math.Clamp(tuple.Item1, 0, 1440);
                var seconds = Math.Clamp(tuple.Item2, 0, 59);
                var milliseconds = Math.Clamp(tuple.Item3, 0, 999);
                milliseconds = (milliseconds / 10) * 10;
                var clamped = ClampSceneTimestamp(TimeSpan.FromMinutes(minutes)
                                                  + TimeSpan.FromSeconds(seconds)
                                                  + TimeSpan.FromMilliseconds(milliseconds));
                SceneTimestamp = clamped;
                SceneTimestampMinutes = (int)clamped.TotalMinutes;
                SceneTimestampSeconds = clamped.Seconds;
                SceneTimestampMilliseconds = (clamped.Milliseconds / 10) * 10;
                _isUpdatingSceneTimestamp = false;
            });
    }

    private static TimeSpan ClampSceneTimestamp(TimeSpan value)
    {
        if (value < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        if (value > MaxSceneTimestamp)
        {
            return MaxSceneTimestamp;
        }

        return value;
    }

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
