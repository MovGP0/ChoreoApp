using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class ExpanderRotateAngleConverter : IValueConverter
{
    public static readonly ExpanderRotateAngleConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double factor = 1.0;
        if (parameter is not null && !double.TryParse(parameter.ToString(), out factor))
        {
            factor = 1.0;
        }

        var direction = GetDirection(value);
        return direction switch
        {
            "Left" => 90 * factor,
            "Right" => -90 * factor,
            _ => 0d
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string? GetDirection(object? value)
    {
        return value switch
        {
            null => null,
            string text => text,
            Enum enumValue => enumValue.ToString(),
            _ => value.ToString()
        };
    }
}
