using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class TextBoxHorizontalScrollBarMarginConverter : IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 9)
        {
            return new Thickness(0);
        }

        if (!TryGetDouble(values[0], out var leadingIconWidth)
            || !TryGetThickness(values[1], out var leadingIconMargin)
            || !TryGetDouble(values[2], out var prefixTextWidth)
            || !TryGetThickness(values[3], out var prefixTextMargin)
            || !TryGetBool(values[4], out var isMouseOver)
            || !TryGetBool(values[5], out var hasKeyboardFocus)
            || !TryGetBool(values[6], out var hasOutlinedTextField)
            || !TryGetThickness(values[7], out var normalBorder)
            || !TryGetThickness(values[8], out var activeBorder))
        {
            return new Thickness(0);
        }

        double iconMargin = leadingIconWidth > 0 ? leadingIconMargin.Left + leadingIconMargin.Right : 0;
        double prefixMargin = prefixTextWidth > 0 ? prefixTextMargin.Left + prefixTextMargin.Right : 0;
        double offset = leadingIconWidth + iconMargin + prefixTextWidth + prefixMargin;
        double bottomOffset = 0;
        double topOffset = 0;

        if (hasOutlinedTextField && (isMouseOver || hasKeyboardFocus))
        {
            double horizDelta = activeBorder.Left - normalBorder.Left;
            double vertDeltaTop = activeBorder.Top - normalBorder.Top;
            double vertDeltaBottom = activeBorder.Bottom - normalBorder.Bottom;
            offset -= horizDelta;
            topOffset += vertDeltaTop;
            bottomOffset -= vertDeltaBottom;
        }

        return new Thickness(offset, topOffset, 0, bottomOffset);
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
}
