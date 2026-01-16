using System.Globalization;

namespace MaterialDesignDemo.Maui.Trees;

public sealed class IntToIndentConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var depth = value is int intValue ? intValue : 0;
        return new Thickness(depth * 16, 0, 0, 0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
