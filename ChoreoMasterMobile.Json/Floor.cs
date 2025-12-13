using System.Text.Json.Serialization;

namespace ChoreoMasterMobile.Json;

public sealed class Floor
{
    [JsonPropertyName("SizeFront")]
    public int SizeFront { get; set; }

    [JsonPropertyName("SizeBack")]
    public int SizeBack { get; set; }

    [JsonPropertyName("SizeLeft")]
    public int SizeLeft { get; set; }

    [JsonPropertyName("SizeRight")]
    public int SizeRight { get; set; }
}
