using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class FloatingHintMarginConverter : IMultiValueConverter
{
    public static readonly FloatingHintMarginConverter Instance = new();

    private static readonly Thickness EmptyThickness = new(0);

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 9)
        {
            return EmptyThickness;
        }

        if (!TryGetBool(values[0], out var isFloatingHint)
            || !TryGetBool(values[1], out var isKeyboardFocusWithin)
            || !TryGetBool(values[2], out var isEditable)
            || !TryGetDouble(values[3], out var prefixWidth)
            || !TryGetThickness(values[4], out var prefixMargin)
            || !TryGetDouble(values[5], out var suffixWidth)
            || !TryGetThickness(values[6], out var suffixMargin))
        {
            return EmptyThickness;
        }

        bool isPrefixAlwaysVisible = IsVisibilityAlways(values[7]);
        bool isSuffixAlwaysVisible = IsVisibilityAlways(values[8]);

        double prefixTotalWidth = prefixWidth > 0 ? prefixWidth + prefixMargin.Right : 0;
        double suffixTotalWidth = suffixWidth > 0 ? suffixWidth + suffixMargin.Left : 0;

        return new Thickness(GetLeftMargin(), 0, GetRightMargin(), 0);

        double GetLeftMargin()
        {
            if (isPrefixAlwaysVisible)
            {
                return prefixWidth + prefixMargin.Right;
            }

            return (isFloatingHint && isEditable) || (!isKeyboardFocusWithin && isEditable)
                ? 0
                : prefixTotalWidth;
        }

        double GetRightMargin()
        {
            if (isSuffixAlwaysVisible)
            {
                return suffixWidth + suffixMargin.Left;
            }

            return (isFloatingHint && isEditable) || (!isKeyboardFocusWithin && isEditable)
                ? 0
                : suffixTotalWidth;
        }
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

    private static bool TryGetThickness(object? value, out Thickness thickness)
    {
        if (value is Thickness result)
        {
            thickness = result;
            return true;
        }

        thickness = new Thickness(0);
        return false;
    }

    private static bool TryGetBool(object? value, out bool result)
    {
        if (value is bool boolValue)
        {
            result = boolValue;
            return true;
        }

        if (value is int intValue)
        {
            result = intValue != 0;
            return true;
        }

        if (value is Enum enumValue)
        {
            result = string.Equals(enumValue.ToString(), "True", StringComparison.OrdinalIgnoreCase);
            return true;
        }

        if (value is string text && bool.TryParse(text, out var parsed))
        {
            result = parsed;
            return true;
        }

        result = false;
        return false;
    }

    private static bool IsVisibilityAlways(object? value)
    {
        return value switch
        {
            bool boolValue => boolValue,
            int intValue => intValue != 0,
            Enum enumValue => string.Equals(enumValue.ToString(), "Always", StringComparison.Ordinal),
            string text => string.Equals(text, "Always", StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}
