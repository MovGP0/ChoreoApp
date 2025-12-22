using System.Globalization;

namespace ChoreoApp.Styling.Converters.Internal;

public sealed class AutomationPropertiesNameConverter : IValueConverter
{
    public static readonly AutomationPropertiesNameConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value as string ?? string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
