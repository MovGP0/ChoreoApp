namespace ChoreoApp.Models;

public sealed partial class FloorModel : IEquatable<FloorModel>
{
    public bool Equals(FloorModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return SizeFront == other.SizeFront
            && SizeBack == other.SizeBack
            && SizeLeft == other.SizeLeft
            && SizeRight == other.SizeRight;
    }

    public override bool Equals(object? obj) => Equals(obj as FloorModel);

    public override int GetHashCode()
    {
        return HashCode.Combine(SizeFront, SizeBack, SizeLeft, SizeRight);
    }
}
