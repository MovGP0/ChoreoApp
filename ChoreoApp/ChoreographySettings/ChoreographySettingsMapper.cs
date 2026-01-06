using System.Globalization;
using ChoreoApp.Models;

namespace ChoreoApp.ChoreographySettings;

public sealed class ChoreographySettingsMapper
{
    public void Map(ChoreographyModel source, ChoreographySettingsViewModel target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        target.Comment = source.Comment ?? string.Empty;
        target.Name = source.Name;
        target.Subtitle = source.Subtitle ?? string.Empty;
        target.Variation = source.Variation ?? string.Empty;
        target.Author = source.Author ?? string.Empty;
        target.Description = source.Description ?? string.Empty;
        target.Date = ParseDate(source.Date);

        var settings = source.Settings;
        target.GridResolution = ClampGridResolution(settings.Resolution);
        target.Transparency = ClampTransparency(settings.Transparency);
        target.PositionsAtSide = settings.PositionsAtSide;
        target.GridLines = settings.GridLines;
        target.FloorColor = settings.FloorColor;
        target.ShowTimestamps = settings.ShowTimestamps;
        if (source.Floor is not null)
        {
            target.FloorFront = ClampFloorSize(source.Floor.SizeFront);
            target.FloorBack = ClampFloorSize(source.Floor.SizeBack);
            target.FloorLeft = ClampFloorSize(source.Floor.SizeLeft);
            target.FloorRight = ClampFloorSize(source.Floor.SizeRight);
        }
    }

    public void Map(ChoreographySettingsViewModel source, ChoreographyModel target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        target.Comment = NormalizeText(source.Comment);
        target.Name = source.Name;
        target.Subtitle = NormalizeText(source.Subtitle);
        target.Variation = NormalizeText(source.Variation);
        target.Author = NormalizeText(source.Author);
        target.Description = NormalizeText(source.Description);
        target.Date = source.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var settings = target.Settings;
        settings.Resolution = ClampGridResolution(source.GridResolution);
        settings.Transparency = ClampTransparency(source.Transparency);
        settings.PositionsAtSide = source.PositionsAtSide;
        settings.GridLines = source.GridLines;
        settings.FloorColor = source.FloorColor;
        settings.ShowTimestamps = source.ShowTimestamps;
        target.Floor.SizeFront = ClampFloorSize(source.FloorFront);
        target.Floor.SizeBack = ClampFloorSize(source.FloorBack);
        target.Floor.SizeLeft = ClampFloorSize(source.FloorLeft);
        target.Floor.SizeRight = ClampFloorSize(source.FloorRight);
    }

    private static int ClampFloorSize(int value) => Math.Clamp(value, 0, 100);

    private static int ClampGridResolution(int value) => Math.Clamp(value, 1, 16);

    private static decimal ClampTransparency(decimal value) => Math.Clamp(value, 0m, 1m);

    private static string? NormalizeText(string value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    private static DateTime ParseDate(string? raw)
    {
        if (!string.IsNullOrWhiteSpace(raw)
            && DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed))
        {
            return parsed.Date;
        }

        return DateTime.Today;
    }
}
