using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.CircularProgressBar;

public sealed class RotateTransformCentreConverter : IValueConverter
{
    public static readonly RotateTransformCentreConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is double width ? width / 2 : 0d;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
