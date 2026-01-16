using ChoreoMasterMobile.Json;

namespace ChoreoApp.Models;

public sealed partial class SettingsModel : ReactiveObject
{
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
    private bool _positionsAtSide = true;

    [Reactive]
    private bool _gridLines;

    [Reactive]
    private bool _snapToGrid = true;

    [Reactive]
    private Color _floorColor = Colors.Transparent;

    [Reactive]
    private decimal _dancerSize;

    [Reactive]
    private bool _showTimestamps = true;

    [Reactive]
    private string? _musicPathAbsolute;

    [Reactive]
    private string? _musicPathRelative;
}
