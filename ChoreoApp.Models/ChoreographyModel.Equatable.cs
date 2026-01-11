namespace ChoreoApp.Models;

public sealed partial class ChoreographyModel : IEquatable<ChoreographyModel>
{
    public bool Equals(ChoreographyModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Comment, other.Comment, StringComparison.Ordinal)
            && Equals(Settings, other.Settings)
            && Equals(Floor, other.Floor)
            && Roles.SequenceEqual(other.Roles)
            && Dancers.SequenceEqual(other.Dancers)
            && Scenes.SequenceEqual(other.Scenes)
            && string.Equals(Name, other.Name, StringComparison.Ordinal)
            && string.Equals(Subtitle, other.Subtitle, StringComparison.Ordinal)
            && string.Equals(Date, other.Date, StringComparison.Ordinal)
            && string.Equals(Variation, other.Variation, StringComparison.Ordinal)
            && string.Equals(Author, other.Author, StringComparison.Ordinal)
            && string.Equals(Description, other.Description, StringComparison.Ordinal)
            && LastSaveDate.Equals(other.LastSaveDate);
    }

    public override bool Equals(object? obj) => Equals(obj as ChoreographyModel);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Comment, StringComparer.Ordinal);
        hash.Add(Settings);
        hash.Add(Floor);
        foreach (var role in Roles)
        {
            hash.Add(role);
        }

        foreach (var dancer in Dancers)
        {
            hash.Add(dancer);
        }

        foreach (var scene in Scenes)
        {
            hash.Add(scene);
        }

        hash.Add(Name, StringComparer.Ordinal);
        hash.Add(Subtitle, StringComparer.Ordinal);
        hash.Add(Date, StringComparer.Ordinal);
        hash.Add(Variation, StringComparer.Ordinal);
        hash.Add(Author, StringComparer.Ordinal);
        hash.Add(Description, StringComparer.Ordinal);
        hash.Add(LastSaveDate);
        return hash.ToHashCode();
    }
}
