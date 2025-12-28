using System.Text.Json;
using System.Globalization;

namespace ChoreoMasterMobile.Json;

public sealed class DancerIdSystemTextJsonConverter : global::System.Text.Json.Serialization.JsonConverter<DancerId>
{
    public override DancerId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new (reader.GetInt32());

    public override void Write(Utf8JsonWriter writer, DancerId value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.Value);

    public override DancerId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(int.Parse(reader.GetString() ?? throw new FormatException("The string for the DancerId property was null")));

    public override void WriteAsPropertyName(Utf8JsonWriter writer, DancerId value, JsonSerializerOptions options)
        => writer.WritePropertyName(value.Value.ToString(CultureInfo.InvariantCulture));
}
