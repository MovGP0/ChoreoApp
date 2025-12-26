using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters;

public sealed class NullableToVisibilityConverter : IValueConverter
{
    public static readonly NullableToVisibilityConverter Instance = new();
    public static readonly NullableToVisibilityConverter CollapsedInstance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return false;
        }

        if (value is string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        return true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
