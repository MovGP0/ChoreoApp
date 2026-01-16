using System.Globalization;

namespace MaterialDesignDemo.Maui.Transitions;

public sealed class IndexEqualsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var index = value is int intValue ? intValue : -1;
        var target = parameter is string text && int.TryParse(text, out var parsed)
            ? parsed
            : -1;
        return index == target;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
