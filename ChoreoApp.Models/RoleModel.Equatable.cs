namespace ChoreoApp.Models;

public sealed partial class RoleModel : IEquatable<RoleModel>
{
    public bool Equals(RoleModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ZIndex == other.ZIndex
            && string.Equals(Name, other.Name, StringComparison.Ordinal)
            && Color.Equals(other.Color);
    }

    public override bool Equals(object? obj) => Equals(obj as RoleModel);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(ZIndex);
        hash.Add(Name, StringComparer.Ordinal);
        hash.Add(Color);
        return hash.ToHashCode();
    }
}
