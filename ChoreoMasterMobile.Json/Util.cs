using System.IO;
using System.Text;
using System.Text.Json;
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

    public static Choreography Import(string json)
    {
        return JsonSerializer.Deserialize<Choreography>(json, Options)
               ?? new Choreography();
    }

    public static Choreography ImportFromFile(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return JsonSerializer.Deserialize<Choreography>(stream, Options)
               ?? new Choreography();
    }

    public static string Export(Choreography choreography)
    {
        return JsonSerializer.Serialize(choreography, Options);
    }

    public static void ExportToFile(string filePath, Choreography choreography, Encoding? encoding = null)
    {
        var json = Export(choreography);
        File.WriteAllText(filePath, json, encoding ?? Encoding.UTF8);
    }
}
