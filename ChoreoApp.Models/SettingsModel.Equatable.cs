namespace ChoreoApp.Models;

public sealed partial class SettingsModel : IEquatable<SettingsModel>
{
    public bool Equals(SettingsModel? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AnimationMilliseconds == other.AnimationMilliseconds
            && FrontPosition == other.FrontPosition
            && DancerPosition == other.DancerPosition
            && Resolution == other.Resolution
            && Transparency == other.Transparency
            && PositionsAtSide == other.PositionsAtSide
            && GridLines == other.GridLines
            && SnapToGrid == other.SnapToGrid
            && FloorColor.Equals(other.FloorColor)
            && DancerSize == other.DancerSize
            && ShowTimestamps == other.ShowTimestamps
            && string.Equals(MusicPathAbsolute, other.MusicPathAbsolute, StringComparison.Ordinal)
            && string.Equals(MusicPathRelative, other.MusicPathRelative, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => Equals(obj as SettingsModel);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(AnimationMilliseconds);
        hash.Add(FrontPosition);
        hash.Add(DancerPosition);
        hash.Add(Resolution);
        hash.Add(Transparency);
        hash.Add(PositionsAtSide);
        hash.Add(GridLines);
        hash.Add(SnapToGrid);
        hash.Add(FloorColor);
        hash.Add(DancerSize);
        hash.Add(ShowTimestamps);
        hash.Add(MusicPathAbsolute, StringComparer.Ordinal);
        hash.Add(MusicPathRelative, StringComparer.Ordinal);
        return hash.ToHashCode();
    }
}
