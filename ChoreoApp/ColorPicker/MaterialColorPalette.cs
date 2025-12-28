namespace ChoreoApp.ColorPicker;

public static class MaterialColorPalette
{
    private static readonly int[] ShadeStops = [ 50, 100, 200, 300, 400, 500, 600, 700, 800, 900 ];

    private static readonly (string BaseName, string DisplayName)[] Groups =
    [
        ("Red", "Red"),
        ("Green", "Green"),
        ("Blue", "Blue"),
        ("Pink", "Pink"),
        ("Purple", "Purple"),
        ("DeepPurple", "Deep Purple"),
        ("Indigo", "Indigo"),
        ("BlueGrey", "Blue Grey"),
        ("Cyan", "Cyan"),
        ("Teal", "Teal"),
        ("LightGreen", "Light Green"),
        ("Lime", "Lime"),
        ("Yellow", "Yellow"),
        ("Amber", "Amber"),
        ("Orange", "Orange"),
        ("DeepOrange", "Deep Orange"),
        ("Brown", "Brown"),
        ("Gray", "Gray")
    ];

    public static IReadOnlyList<MaterialColorGroup> BuildGroups(ResourceDictionary? resources = null)
    {
        resources ??= Application.Current?.Resources;
        if (resources is null)
        {
            return [];
        }

        var result = new List<MaterialColorGroup>();
        foreach (var (baseName, displayName) in Groups)
        {
            var group = new MaterialColorGroup(displayName);
            foreach (var shade in ShadeStops)
            {
                var key = $"{baseName}{shade}";
                if (resources.TryGetValue(key, out var value) && value is Color color)
                {
                    group.Add(new MaterialColorOption(key, $"{displayName} {shade}", color));
                }
            }

            if (group.Count > 0)
            {
                result.Add(group);
            }
        }

        return result;
    }
}
