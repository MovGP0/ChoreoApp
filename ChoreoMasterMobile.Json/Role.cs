using System.Text.Json.Serialization;
using Color = Microsoft.Maui.Graphics.Color;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoMasterMobile.Json;

public sealed class Role
{
    [JsonPropertyName("ZIndex")]
    public int ZIndex { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Color")]
    [JsonConverter(typeof(ColorHexJsonConverter))]
    public Color Color { get; set; }
}
