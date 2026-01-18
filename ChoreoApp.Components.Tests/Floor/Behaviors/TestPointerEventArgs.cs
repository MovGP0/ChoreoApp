namespace ChoreoApp.Components.Tests.Floor.Behaviors;

internal sealed class TestPointerEventArgs : PointerEventArgs
{
    private readonly Point _point;

    public TestPointerEventArgs(Point point)
    {
        _point = point;
    }

    public override Point? GetPosition(Element? relativeTo)
    {
        return _point;
    }
}
