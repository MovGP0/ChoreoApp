using System.Text.Json.Serialization;

namespace ChoreoMasterMobile.Json;

public sealed class Choreography
{
    [JsonPropertyName("_Comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("Settings")]
    public Settings Settings { get; set; } = new();

    [JsonPropertyName("Floor")]
    public Floor Floor { get; set; } = new();

    [JsonPropertyName("Roles")]
    public List<Role> Roles { get; set; } = new();

    [JsonPropertyName("Dancers")]
    public List<Dancer> Dancers { get; set; } = new();

    [JsonPropertyName("Scenes")]
    public List<Scene> Scenes { get; set; } = new();

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Subtitle")]
    public string? Subtitle { get; set; }

    [JsonPropertyName("Date")]
    public string? Date { get; set; }

    [JsonPropertyName("Variation")]
    public string? Variation { get; set; }

    [JsonPropertyName("Author")]
    public string? Author { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("LastSaveDate")]
    public DateTimeOffset LastSaveDate { get; set; } = DateTimeOffset.UtcNow;
}
