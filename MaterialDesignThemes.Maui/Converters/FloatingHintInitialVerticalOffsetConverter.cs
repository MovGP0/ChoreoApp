using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters;

public sealed class FloatingHintInitialVerticalOffsetConverter : IMultiValueConverter
{
    public static readonly FloatingHintInitialVerticalOffsetConverter Instance = new();

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 3)
        {
            return 0d;
        }

        if (!TryGetDouble(values[0], out var contentHostHeight)
            || !TryGetDouble(values[1], out var hintHeight)
            || !TryGetInt(values[2], out var lineCount))
        {
            return 0d;
        }

        double offsetMultiplier = 0d;
        if (lineCount > 1)
        {
            offsetMultiplier = lineCount / 2d - 0.5d;
        }

        return Math.Max(0, (contentHostHeight - hintHeight) / 2 - (offsetMultiplier * hintHeight));
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
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

    private static bool TryGetInt(object? value, out int result)
    {
        if (value is int intValue)
        {
            result = intValue;
            return true;
        }

        if (value is double doubleValue)
        {
            result = (int)doubleValue;
            return true;
        }

        result = 0;
        return false;
    }
}
