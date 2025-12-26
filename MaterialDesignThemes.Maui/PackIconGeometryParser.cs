using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

internal static class PackIconGeometryParser
{
    public static Geometry? Parse(PackIconKind kind)
    {
        if (!PackIconIconData.TryGetData(kind, out var data) || string.IsNullOrWhiteSpace(data))
        {
            return null;
        }

        return Geometry.Parse(data);
    }
}
