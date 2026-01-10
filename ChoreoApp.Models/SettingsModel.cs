using ChoreoMasterMobile.Json;

namespace ChoreoApp.Models;

public sealed partial class SettingsModel : ReactiveObject
{
    public SettingsModel()
    {
        ShowTimestamps = Preferences.Default.Get(SettingsPreferenceKeys.ShowTimestamps, true);
        PositionsAtSide = Preferences.Default.Get(SettingsPreferenceKeys.PositionsAtSide, true);
    }

    [Reactive]
    private int _animationMilliseconds;

    [Reactive]
    private FrontPosition _frontPosition;

    [Reactive]
    private FrontPosition _dancerPosition;

    [Reactive]
    private int _resolution;

    [Reactive]
    private decimal _transparency;

    [Reactive]
    private bool _positionsAtSide;

    [Reactive]
    private bool _gridLines;

    [Reactive]
    private Color _floorColor = Colors.Transparent;

    [Reactive]
    private decimal _dancerSize;

    [Reactive]
    private bool _showTimestamps;

    [Reactive]
    private string? _musicPathAbsolute;

    [Reactive]
    private string? _musicPathRelative;
}
