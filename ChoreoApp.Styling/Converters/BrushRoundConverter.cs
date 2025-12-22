using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class BrushRoundConverter : IValueConverter
{
    public static readonly BrushRoundConverter Instance = new();

    public Color HighValue { get; set; } = Colors.White;
    public Color LowValue { get; set; } = Colors.Black;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = value switch
        {
            SolidColorBrush solidColorBrush => solidColorBrush.Color,
            Color c => c,
            _ => (Color?)null
        };

        if (color is null)
        {
            return null;
        }

        var luminance = 0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue;
        return luminance >= 0.5 ? HighValue : LowValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
