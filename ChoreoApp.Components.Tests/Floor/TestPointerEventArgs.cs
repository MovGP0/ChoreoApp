namespace ChoreoApp.Components.Tests.Floor;

internal sealed class TestPointerEventArgs(Point point) : PointerEventArgs
{
    public override Point? GetPosition(Element? relativeTo) => point;
}
