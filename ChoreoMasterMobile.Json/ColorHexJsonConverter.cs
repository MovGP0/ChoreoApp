using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Maui.Graphics;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoMasterMobile.Json;

/// <summary>
/// Converts colors to/from #AARRGGBB hex strings used in the .choreo export files.
/// </summary>
public sealed class ColorHexJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return Colors.Transparent;
        }

        try
        {
            // MAUI Color already understands #RGB, #RRGGBB, #AARRGGBB, and named colors.
            return Color.Parse(value);
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException or FormatException)
        {
            throw new JsonException($"Invalid color format: {value}", ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        // Color channels are floats in 0..1; convert back to #AARRGGBB
        byte a = ToByte(value.Alpha);
        byte r = ToByte(value.Red);
        byte g = ToByte(value.Green);
        byte b = ToByte(value.Blue);

        string hex = $"#{a:X2}{r:X2}{g:X2}{b:X2}";
        writer.WriteStringValue(hex);
    }

    private static byte ToByte(float channel)
    {
        return (byte)Math.Clamp((int)Math.Round(channel * 255f), 0, 255);
    }
}
