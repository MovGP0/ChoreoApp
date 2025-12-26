using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters;

public sealed class DoubleToIntConverter : IValueConverter
{
    public static readonly DoubleToIntConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return 0;
        }

        try
        {
            var doubleValue = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);

            if (doubleValue <= 0)
            {
                return 0;
            }

            return (int)Math.Round(doubleValue, MidpointRounding.AwayFromZero);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            return 0;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
