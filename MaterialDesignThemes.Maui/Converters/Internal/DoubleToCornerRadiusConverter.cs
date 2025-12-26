using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class DoubleToCornerRadiusConverter : IValueConverter
{
    public static readonly DoubleToCornerRadiusConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var radius = value is double d ? Math.Max(0, d) : 0;
        return new CornerRadius(radius);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
