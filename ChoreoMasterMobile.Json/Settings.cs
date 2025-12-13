using System.Text.Json.Serialization;
using Color = Microsoft.Maui.Graphics.Color;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoMasterMobile.Json;

public sealed class Settings
{
    [JsonPropertyName("AnimationMilliseconds")]
    public int AnimationMilliseconds { get; set; }

    [JsonPropertyName("FrontPosition")]
    public FrontPosition FrontPosition { get; set; }

    [JsonPropertyName("DancerPosition")]
    public FrontPosition DancerPosition { get; set; }

    [JsonPropertyName("Resolution")]
    public int Resolution { get; set; }

    [JsonPropertyName("Transparency")]
    public decimal Transparency { get; set; }

    [JsonPropertyName("PositionsAtSide")]
    public bool PositionsAtSide { get; set; }

    [JsonPropertyName("GridLines")]
    public bool GridLines { get; set; }

    [JsonPropertyName("FloorColor")]
    [JsonConverter(typeof(ColorHexJsonConverter))]
    public Color FloorColor { get; set; } = Colors.Transparent;

    [JsonPropertyName("DancerSize")]
    public decimal DancerSize { get; set; }

    [JsonPropertyName("ShowTimestamps")]
    public bool ShowTimestamps { get; set; }

    [JsonPropertyName("MusicPathAbsolute")]
    public string? MusicPathAbsolute { get; set; }

    [JsonPropertyName("MusicPathRelative")]
    public string? MusicPathRelative { get; set; }
}
