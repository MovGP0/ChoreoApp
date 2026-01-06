namespace ChoreoApp.Models;

public sealed partial class FloorModel : ReactiveObject
{
    [Reactive]
    private int _sizeFront;

    [Reactive]
    private int _sizeBack;

    [Reactive]
    private int _sizeLeft;

    [Reactive]
    private int _sizeRight;
}
