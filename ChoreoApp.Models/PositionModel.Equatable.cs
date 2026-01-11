namespace ChoreoApp.Models;

public sealed partial class PositionModel : IEquatable<PositionModel>
{
    public bool Equals(PositionModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Equals(Dancer, other.Dancer)
            && Nullable.Equals(Orientation, other.Orientation)
            && X.Equals(other.X)
            && Y.Equals(other.Y)
            && Nullable.Equals(Curve1X, other.Curve1X)
            && Nullable.Equals(Curve1Y, other.Curve1Y)
            && Nullable.Equals(Curve2X, other.Curve2X)
            && Nullable.Equals(Curve2Y, other.Curve2Y)
            && Nullable.Equals(Movement1X, other.Movement1X)
            && Nullable.Equals(Movement1Y, other.Movement1Y)
            && Nullable.Equals(Movement2X, other.Movement2X)
            && Nullable.Equals(Movement2Y, other.Movement2Y);
    }

    public override bool Equals(object? obj) => Equals(obj as PositionModel);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Dancer);
        hash.Add(Orientation);
        hash.Add(X);
        hash.Add(Y);
        hash.Add(Curve1X);
        hash.Add(Curve1Y);
        hash.Add(Curve2X);
        hash.Add(Curve2Y);
        hash.Add(Movement1X);
        hash.Add(Movement1Y);
        hash.Add(Movement2X);
        hash.Add(Movement2Y);
        return hash.ToHashCode();
    }
}
