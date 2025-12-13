using System.Text.Json.Serialization;

namespace ChoreoMasterMobile.Json;

public sealed class Position
{
    [JsonPropertyName("Dancer")]
    public Dancer Dancer { get; set; } = null!;

    [JsonPropertyName("O")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Orientation { get; set; }

    [JsonPropertyName("X")]
    public double X { get; set; }

    [JsonPropertyName("Y")]
    public double Y { get; set; }

    [JsonPropertyName("BX")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve1X { get; set; }

    [JsonPropertyName("BY")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve1Y { get; set; }

    [JsonPropertyName("CX")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve2X { get; set; }

    [JsonPropertyName("CY")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve2Y { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement1X { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement1Y { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement2X { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement2Y { get; set; }
}
