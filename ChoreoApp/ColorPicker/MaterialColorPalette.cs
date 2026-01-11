using ChoreoApp.i18n;

namespace ChoreoApp.ColorPicker;

public static class MaterialColorPalette
{
    private static readonly Lazy<IReadOnlyList<MaterialColorGroup>> s_defaultGroups = new(() => BuildGroups());
    private static readonly int[] ShadeStops = [ 50, 100, 200, 300, 400, 500, 600, 700, 800, 900 ];

    private static readonly (string BaseName, Func<string> DisplayNameFactory)[] Groups =
    [
        ("Red", () => Translations.ColorRed),
        ("Green", () => Translations.ColorGreen),
        ("Blue", () => Translations.ColorBlue),
        ("Pink", () => Translations.ColorPink),
        ("Purple", () => Translations.ColorPurple),
        ("DeepPurple", () => Translations.ColorDeepPurple),
        ("Indigo", () => Translations.ColorIndigo),
        ("BlueGrey", () => Translations.ColorBlueGrey),
        ("Cyan", () => Translations.ColorCyan),
        ("Teal", () => Translations.ColorTeal),
        ("LightGreen", () => Translations.ColorLightGreen),
        ("Lime", () => Translations.ColorLime),
        ("Yellow", () => Translations.ColorYellow),
        ("Amber", () => Translations.ColorAmber),
        ("Orange", () => Translations.ColorOrange),
        ("DeepOrange", () => Translations.ColorDeepOrange),
        ("Brown", () => Translations.ColorBrown),
        ("Gray", () => Translations.ColorGray)
    ];

    public static IReadOnlyList<MaterialColorGroup> DefaultGroups => s_defaultGroups.Value;

    public static IReadOnlyList<MaterialColorGroup> BuildGroups(ResourceDictionary? resources = null)
    {
        resources ??= Application.Current?.Resources;
        if (resources is null)
        {
            return [];
        }

        var result = new List<MaterialColorGroup>();
        foreach (var (baseName, displayNameFactory) in Groups)
        {
            var displayName = displayNameFactory();
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
