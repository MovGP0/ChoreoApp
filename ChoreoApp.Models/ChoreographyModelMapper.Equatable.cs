namespace ChoreoApp.Models;

public sealed partial class ChoreographyModelMapper : IEquatable<ChoreographyModelMapper>
{
    public bool Equals(ChoreographyModelMapper? other)
    {
        return other is not null;
    }

    public override bool Equals(object? obj) => Equals(obj as ChoreographyModelMapper);

    public override int GetHashCode()
    {
        return typeof(ChoreographyModelMapper).GetHashCode();
    }
}
