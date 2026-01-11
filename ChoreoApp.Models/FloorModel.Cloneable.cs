namespace ChoreoApp.Models;

public sealed partial class FloorModel : ICloneable<FloorModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public FloorModel Clone(CloneMode mode)
    {
        return new FloorModel
        {
            SizeFront = SizeFront,
            SizeBack = SizeBack,
            SizeLeft = SizeLeft,
            SizeRight = SizeRight
        };
    }
}
