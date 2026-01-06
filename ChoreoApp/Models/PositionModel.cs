namespace ChoreoApp.Models;

public sealed partial class PositionModel : ReactiveObject
{
    [Reactive]
    private DancerModel? _dancer;

    [Reactive]
    private double? _orientation;

    [Reactive]
    private double _x;

    [Reactive]
    private double _y;

    [Reactive]
    private double? _curve1X;

    [Reactive]
    private double? _curve1Y;

    [Reactive]
    private double? _curve2X;

    [Reactive]
    private double? _curve2Y;

    [Reactive]
    private double? _movement1X;

    [Reactive]
    private double? _movement1Y;

    [Reactive]
    private double? _movement2X;

    [Reactive]
    private double? _movement2Y;
}
