namespace ChoreoApp.Models;

public sealed partial class ChoreographyModelMapper : ICloneable<ChoreographyModelMapper>
{
    public object Clone() => Clone(CloneMode.Deep);

    public ChoreographyModelMapper Clone(CloneMode mode)
    {
        return new ChoreographyModelMapper();
    }
}
