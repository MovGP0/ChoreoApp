namespace ChoreoApp.Styling;

internal static class PackIconIconData
{
    private static readonly Lazy<IDictionary<PackIconKind, string>> Data =
        new(PackIconDataFactory.Create);

    public static bool TryGetData(PackIconKind kind, out string data) =>
        Data.Value.TryGetValue(kind, out data!);
}
