using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class BooleanToDashStyleConverter : IValueConverter
{
    public object? TrueValue { get; set; }
    public object? FalseValue { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool flag && flag ? TrueValue : FalseValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, TrueValue);
    }
}
