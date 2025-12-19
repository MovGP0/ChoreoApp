using System.Text.Json.Serialization;
using Color = Microsoft.Maui.Graphics.Color;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoMasterMobile.Json;

public sealed class Scene
{
    [JsonIgnore]
    public int SceneId { get; set; }

    [JsonPropertyName("Positions")]
    public IList<Position>? Positions { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Text")]
    public string? Text { get; set; }

    [JsonPropertyName("FixedPositions")]
    public bool FixedPositions { get; set; }

    [JsonPropertyName("Timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimeSpan? Timestamp { get; set; }

    [JsonPropertyName("VariationDepth")]
    public int VariationDepth { get; set; }

    [JsonPropertyName("Variations")]
    public IList<IList<Scene>>? Variations { get; set; }

    [JsonPropertyName("CurrentVariation")]
    public IList<Scene>? CurrentVariation { get; set; }

    [JsonPropertyName("Color")]
    [JsonConverter(typeof(ColorHexJsonConverter))]
    public Color Color { get; set; } = Colors.Transparent;
}
