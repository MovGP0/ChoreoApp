using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ChoreoMasterMobile.Json;

/// <summary>
/// Helper methods for reading and writing .choreo export files using System.Text.Json with reference preservation.
/// </summary>
public static class Util
{
    private static readonly JsonSerializerOptions Options = new()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        PropertyNamingPolicy = null,
        WriteIndented = true
    };

    private static readonly ChoreographyJsonContext Context = new(Options);

    public static Choreography Import(string json)
    {
        var choreography = JsonSerializer.Deserialize(json, Context.Choreography)
                           ?? new Choreography();
        ApplySceneIds(choreography, json);
        ApplyDancerIds(choreography, json);
        return choreography;
    }

    public static Choreography ImportFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return Import(json);
    }

    public static string Export(Choreography choreography)
    {
        return JsonSerializer.Serialize(
            choreography,
            Context.Choreography);
    }

    public static void ExportToFile(string filePath, Choreography choreography, Encoding? encoding = null)
    {
        var json = Export(choreography);
        File.WriteAllText(filePath, json, encoding ?? Encoding.UTF8);
    }

    private static void ApplySceneIds(Choreography choreography, string json)
    {
        if (choreography.Scenes.Count == 0)
        {
            return;
        }

        var root = JsonNode.Parse(json) as JsonObject;
        var scenesNode = root?["Scenes"] as JsonArray;
        if (scenesNode is null)
        {
            return;
        }

        int count = Math.Min(choreography.Scenes.Count, scenesNode.Count);
        for (int i = 0; i < count; i++)
        {
            if (scenesNode[i] is not JsonObject sceneNode)
            {
                continue;
            }

            if (!sceneNode.TryGetPropertyValue("$id", out var idNode))
            {
                continue;
            }

            var sceneId = idNode switch
            {
                JsonValue value when value.TryGetValue(out int numberValue) => numberValue,
                JsonValue value when value.TryGetValue(out string? stringValue)
                    && int.TryParse(stringValue, out var parsedValue) => parsedValue,
                _ => 0
            };

            if (sceneId > 0)
            {
                choreography.Scenes[i].SceneId = sceneId;
            }
        }
    }

    private static void ApplyDancerIds(Choreography choreography, string json)
    {
        if (choreography.Dancers.Count == 0)
        {
            return;
        }

        var root = JsonNode.Parse(json) as JsonObject;
        var dancersNode = root?["Dancers"] as JsonArray;
        if (dancersNode is null)
        {
            return;
        }

        int count = Math.Min(choreography.Dancers.Count, dancersNode.Count);
        for (int i = 0; i < count; i++)
        {
            if (dancersNode[i] is not JsonObject dancerNode)
            {
                continue;
            }

            if (!dancerNode.TryGetPropertyValue("$id", out var idNode))
            {
                continue;
            }

            var dancerId = idNode switch
            {
                JsonValue value when value.TryGetValue(out int numberValue) => numberValue,
                JsonValue value when value.TryGetValue(out string? stringValue)
                    && int.TryParse(stringValue, out var parsedValue) => parsedValue,
                _ => 0
            };

            if (dancerId > 0)
            {
                choreography.Dancers[i].DancerId = dancerId;
            }
        }
    }
}
