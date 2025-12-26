using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.CircularProgressBar;

public sealed class StartPointConverter : IValueConverter
{
    public static readonly StartPointConverter Instance = new();

    [Obsolete]
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double v && v > 0.0)
        {
            return new Point(v / 2, 0);
        }

        return new Point();
    }

    [Obsolete]
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
