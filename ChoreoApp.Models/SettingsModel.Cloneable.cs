namespace ChoreoApp.Models;

public sealed partial class SettingsModel : ICloneable<SettingsModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public SettingsModel Clone(CloneMode mode)
    {
        return new SettingsModel
        {
            AnimationMilliseconds = AnimationMilliseconds,
            FrontPosition = FrontPosition,
            DancerPosition = DancerPosition,
            Resolution = Resolution,
            Transparency = Transparency,
            PositionsAtSide = PositionsAtSide,
            GridLines = GridLines,
            FloorColor = FloorColor,
            DancerSize = DancerSize,
            ShowTimestamps = ShowTimestamps,
            MusicPathAbsolute = MusicPathAbsolute,
            MusicPathRelative = MusicPathRelative
        };
    }
}
