using System.Text.Json.Serialization;

namespace ChoreoMasterMobile.Json;

public sealed class Position
{
    /// <summary>
    /// Dancer this position belongs to.
    /// </summary>
    [JsonPropertyName("Dancer")]
    public Dancer Dancer { get; set; } = null!;

    /// <summary>
    /// Optional orientation angle at this position.
    /// </summary>
    [JsonPropertyName("O")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Orientation { get; set; }

    /// <summary>
    /// Absolute X coordinate of the position.
    /// </summary>
    [JsonPropertyName("X")]
    public double X { get; set; }

    /// <summary>
    /// Absolute Y coordinate of the position.
    /// </summary>
    [JsonPropertyName("Y")]
    public double Y { get; set; }

    /// <summary>
    /// Absolute X coordinate of the first Bezier control point (curve handle).
    /// </summary>
    [JsonPropertyName("BX")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve1X { get; set; }

    /// <summary>
    /// Absolute Y coordinate of the first Bezier control point (curve handle).
    /// </summary>
    [JsonPropertyName("BY")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve1Y { get; set; }

    /// <summary>
    /// Absolute X coordinate of the second Bezier control point (curve handle).
    /// </summary>
    [JsonPropertyName("CX")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve2X { get; set; }

    /// <summary>
    /// Absolute Y coordinate of the second Bezier control point (curve handle).
    /// </summary>
    [JsonPropertyName("CY")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve2Y { get; set; }

    /// <summary>
    /// Optional movement X component for this position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement1X { get; set; }

    /// <summary>
    /// Optional movement Y component for this position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement1Y { get; set; }

    /// <summary>
    /// Optional movement X component for this position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement2X { get; set; }

    /// <summary>
    /// Optional movement Y component for this position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Movement2Y { get; set; }
}
