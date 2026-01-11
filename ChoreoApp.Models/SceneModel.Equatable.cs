namespace ChoreoApp.Models;

public sealed partial class SceneModel : IEquatable<SceneModel>
{
    public bool Equals(SceneModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return SceneId.Equals(other.SceneId)
            && string.Equals(Name, other.Name, StringComparison.Ordinal)
            && string.Equals(Text, other.Text, StringComparison.Ordinal)
            && FixedPositions == other.FixedPositions
            && Nullable.Equals(Timestamp, other.Timestamp)
            && VariationDepth == other.VariationDepth
            && Positions.SequenceEqual(other.Positions)
            && VariationsEqual(Variations, other.Variations)
            && CurrentVariation.SequenceEqual(other.CurrentVariation)
            && Color.Equals(other.Color);
    }

    public override bool Equals(object? obj) => Equals(obj as SceneModel);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(SceneId);
        hash.Add(Name, StringComparer.Ordinal);
        hash.Add(Text, StringComparer.Ordinal);
        hash.Add(FixedPositions);
        hash.Add(Timestamp);
        hash.Add(VariationDepth);
        foreach (var position in Positions)
        {
            hash.Add(position);
        }

        foreach (var variation in Variations)
        {
            foreach (var scene in variation)
            {
                hash.Add(scene);
            }
        }

        foreach (var scene in CurrentVariation)
        {
            hash.Add(scene);
        }

        hash.Add(Color);
        return hash.ToHashCode();
    }

    private static bool VariationsEqual(
        IReadOnlyList<IReadOnlyList<SceneModel>> left,
        IReadOnlyList<IReadOnlyList<SceneModel>> right)
    {
        if (left.Count != right.Count)
        {
            return false;
        }

        for (int i = 0; i < left.Count; i++)
        {
            if (!left[i].SequenceEqual(right[i]))
            {
                return false;
            }
        }

        return true;
    }
}
