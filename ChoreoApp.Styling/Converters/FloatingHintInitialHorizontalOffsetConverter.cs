using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class FloatingHintInitialHorizontalOffsetConverter : IMultiValueConverter
{
    public static readonly FloatingHintInitialHorizontalOffsetConverter Instance = new();

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 10)
        {
            return 0d;
        }

        if (!TryGetDouble(values[0], out var prefixWidth)
            || !TryGetThickness(values[1], out var prefixMargin)
            || !TryGetDouble(values[2], out var suffixWidth)
            || !TryGetThickness(values[3], out var suffixMargin)
            || !TryGetLayoutAlignment(values[8], out var horizontalContentAlignment)
            || !TryGetBool(values[9], out var isEditable))
        {
            return 0d;
        }

        bool isPrefixAlwaysVisible = IsVisibilityAlways(values[4]);
        bool isSuffixAlwaysVisible = IsVisibilityAlways(values[5]);
        bool prefixAlignWithText = IsHintAlignedWithText(values[6]);
        bool suffixAlignWithText = IsHintAlignedWithText(values[7]);

        return horizontalContentAlignment switch
        {
            LayoutAlignment.Center => 0d,
            LayoutAlignment.End => GetRightOffset(),
            _ => GetLeftOffset()
        };

        double GetLeftOffset()
        {
            if (!isPrefixAlwaysVisible)
            {
                if (prefixAlignWithText && isEditable)
                {
                    return prefixWidth + prefixMargin.Right;
                }

                if (!prefixAlignWithText && !isEditable && prefixWidth > 0d)
                {
                    return -(prefixWidth + prefixMargin.Right);
                }
            }

            if (isPrefixAlwaysVisible && !prefixAlignWithText)
            {
                return -(prefixWidth + prefixMargin.Right);
            }

            return 0d;
        }

        double GetRightOffset()
        {
            if (!isSuffixAlwaysVisible)
            {
                if (suffixAlignWithText && isEditable)
                {
                    return -(suffixWidth + suffixMargin.Left);
                }

                if (!suffixAlignWithText && !isEditable && suffixWidth > 0d)
                {
                    return suffixWidth + suffixMargin.Left;
                }
            }

            if (isSuffixAlwaysVisible && !suffixAlignWithText)
            {
                return suffixWidth + suffixMargin.Left;
            }

            return 0d;
        }
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

    private static bool TryGetLayoutAlignment(object? value, out LayoutAlignment alignment)
    {
        switch (value)
        {
            case LayoutAlignment layoutAlignment:
                alignment = layoutAlignment;
                return true;
            case LayoutOptions layoutOptions:
                alignment = layoutOptions.Alignment;
                return true;
            case TextAlignment textAlignment:
                alignment = textAlignment switch
                {
                    TextAlignment.Center => LayoutAlignment.Center,
                    TextAlignment.End => LayoutAlignment.End,
                    _ => LayoutAlignment.Start
                };
                return true;
            case string text when Enum.TryParse(text, true, out LayoutAlignment parsed):
                alignment = parsed;
                return true;
            default:
                alignment = LayoutAlignment.Start;
                return false;
        }
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

    private static bool IsHintAlignedWithText(object? value)
    {
        return value switch
        {
            int intValue => intValue == 1,
            Enum enumValue => string.Equals(enumValue.ToString(), "AlignWithText", StringComparison.Ordinal),
            string text => string.Equals(text, "AlignWithText", StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}
