using System.Globalization;
using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class DoubleToRoundRectangleConverter : IValueConverter
{
    public static readonly DoubleToRoundRectangleConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var radius = value is double d ? Math.Max(0, d) : 0;
        return new RoundRectangle { CornerRadius = new CornerRadius(radius) };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
