using System.Globalization;

namespace ChoreoApp.Styling.Converters.Internal;

public sealed class TextBoxHorizontalScrollBarWidthConverter : IMultiValueConverter
{
    public double VerticalScrollBarWidth { get; set; }

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 2)
        {
            return double.NaN;
        }

        if (!TryGetDouble(values[0], out var contentHostWidth))
        {
            return double.NaN;
        }

        bool isVerticalScrollBarVisible = IsVisible(values[1]);
        double scrollBarWidth = VerticalScrollBarWidth;

        if (values.Length > 2 && TryGetDouble(values[2], out var providedWidth))
        {
            scrollBarWidth = providedWidth;
        }

        return Math.Max(0, contentHostWidth - (isVerticalScrollBarVisible ? scrollBarWidth : 0));
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static bool TryGetDouble(object? value, out double result)
    {
        if (value is double doubleValue)
        {
            result = doubleValue;
            return true;
        }

        if (value is float floatValue)
        {
            result = floatValue;
            return true;
        }

        if (value is int intValue)
        {
            result = intValue;
            return true;
        }

        result = 0d;
        return false;
    }

    private static bool IsVisible(object? value)
    {
        return value switch
        {
            bool boolValue => boolValue,
            Enum enumValue => string.Equals(enumValue.ToString(), "Visible", StringComparison.OrdinalIgnoreCase),
            string text => string.Equals(text, "Visible", StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}
