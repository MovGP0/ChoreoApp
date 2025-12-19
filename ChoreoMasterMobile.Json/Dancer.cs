using System.Text.Json.Serialization;
using StronglyTypedIds;
using Color = Microsoft.Maui.Graphics.Color;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoMasterMobile.Json;

[StronglyTypedId(Template.Int)]
public partial struct DancerId;

public sealed class Dancer
{
    [JsonIgnore]
    public DancerId DancerId { get; set; } = DancerId.Empty;

    [JsonPropertyName("Role")]
    public Role Role { get; set; } = null!;

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Shortcut")]
    public string Shortcut { get; set; } = string.Empty;

    [JsonPropertyName("Color")]
    [JsonConverter(typeof(ColorHexJsonConverter))]
    public Color Color { get; set; } = Colors.Transparent;

    [JsonPropertyName("Icon")]
    public string? Icon { get; set; }
}
