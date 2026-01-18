using System.Globalization;

namespace ChoreoApp.ChoreographySettings.Converters;

public sealed class DecimalDoubleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            return (double)decimalValue;
        }

        if (value is double doubleValue)
        {
            return doubleValue;
        }

        return 0d;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            var asDecimal = (decimal)doubleValue;
            var clamped = Math.Clamp(asDecimal, 0m, 1m);
            return decimal.Round(clamped, 2, MidpointRounding.AwayFromZero);
        }

        if (value is decimal decimalValue)
        {
            return Math.Clamp(decimalValue, 0m, 1m);
        }

        return 0m;
    }
}
