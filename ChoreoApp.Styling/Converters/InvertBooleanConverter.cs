using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class InvertBooleanConverter : IValueConverter
{
    public static readonly InvertBooleanConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool flag ? !flag : false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
