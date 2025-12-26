using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters;

public sealed class StringLengthValueConverter : IValueConverter
{
    public static readonly StringLengthValueConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string stringValue ? stringValue.Length : 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
