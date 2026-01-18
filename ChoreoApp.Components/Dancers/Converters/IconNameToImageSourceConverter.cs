using System.Globalization;
using ChoreoApp.i18n;

namespace ChoreoApp.Dancers.Converters;

public sealed class IconNameToImageSourceConverter : IValueConverter
{
    private const string ResourcePrefix = "ChoreoApp.i18n.Icons.";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string iconName || string.IsNullOrWhiteSpace(iconName))
        {
            return null;
        }

        var normalized = NormalizeIconName(iconName);
        var resourceName = $"{ResourcePrefix}{normalized}.png";
        return ImageSource.FromResource(resourceName, typeof(Translations).Assembly);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }

    private static string NormalizeIconName(string iconName)
    {
        var normalized = iconName.Replace('\\', '/');
        if (normalized.StartsWith("Icon", StringComparison.OrdinalIgnoreCase) && normalized.IndexOf('/') < 0)
        {
            normalized = normalized[4..];
        }

        if (normalized.Contains('/'))
        {
            normalized = Path.GetFileNameWithoutExtension(normalized);
        }
        else if (normalized.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            normalized = Path.GetFileNameWithoutExtension(normalized);
        }

        return normalized;
    }
}
