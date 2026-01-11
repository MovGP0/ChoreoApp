namespace ChoreoApp.Models;

public sealed partial class DancerModel : IEquatable<DancerModel>
{
    public bool Equals(DancerModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return DancerId.Equals(other.DancerId)
            && Equals(Role, other.Role)
            && string.Equals(Name, other.Name, StringComparison.Ordinal)
            && string.Equals(Shortcut, other.Shortcut, StringComparison.Ordinal)
            && Color.Equals(other.Color)
            && string.Equals(Icon, other.Icon, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => Equals(obj as DancerModel);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(DancerId);
        hash.Add(Role);
        hash.Add(Name, StringComparer.Ordinal);
        hash.Add(Shortcut, StringComparer.Ordinal);
        hash.Add(Color);
        hash.Add(Icon, StringComparer.Ordinal);
        return hash.ToHashCode();
    }
}
