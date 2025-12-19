using System.Text.Json.Serialization;

namespace ChoreoMasterMobile.Json;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Choreography))]
[JsonSerializable(typeof(Scene))]
[JsonSerializable(typeof(Position))]
[JsonSerializable(typeof(Dancer))]
[JsonSerializable(typeof(Role))]
[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(Floor))]
[JsonSerializable(typeof(FrontPosition))]
internal sealed partial class ChoreographyJsonContext: JsonSerializerContext;
