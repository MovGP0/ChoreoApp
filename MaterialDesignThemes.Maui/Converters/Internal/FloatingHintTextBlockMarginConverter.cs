using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class FloatingHintTextBlockMarginConverter : IMultiValueConverter
{
    public static readonly FloatingHintTextBlockMarginConverter Instance = new();

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 8)
        {
            return new Thickness(0);
        }

        if (!TryGetAlignment(values[2], out var restingAlignment)
            || !TryGetDouble(values[3], out var desiredWidth)
            || !TryGetDouble(values[4], out var availableWidth)
            || !TryGetDouble(values[5], out var scale)
            || !TryGetDouble(values[6], out var lower)
            || !TryGetDouble(values[7], out var upper))
        {
            return new Thickness(0);
        }

        var restingAlignmentOverride = ParseAlignmentOverride(values[0]);
        var floatingAlignment = ParseAlignmentOverride(values[1]);

        double scaleMultiplier = upper + (lower - upper) * scale;

        var restAlignment = ResolveAlignment(restingAlignmentOverride, restingAlignment);
        var floatAlignment = scale != 0 ? ResolveAlignment(floatingAlignment, restingAlignment) : restAlignment;

        double leftThickness = floatAlignment switch
        {
            LayoutAlignment.End => FloatRight(),
            LayoutAlignment.Center => FloatCenter(),
            _ => FloatLeft()
        };

        return new Thickness(Math.Round(leftThickness), 0, 0, 0);

        double FloatLeft()
        {
            if (restAlignment == LayoutAlignment.Center)
            {
                double offset = Math.Max(0, (availableWidth - desiredWidth) / 2);
                return offset - offset * scale;
            }

            if (restAlignment == LayoutAlignment.End)
            {
                double offset = Math.Max(0, availableWidth - desiredWidth);
                return offset - offset * scale;
            }

            return 0;
        }

        double FloatCenter()
        {
            if (restAlignment == LayoutAlignment.Start || restAlignment == LayoutAlignment.Fill)
            {
                double offset = Math.Max(0, (availableWidth - desiredWidth * scaleMultiplier) / 2);
                return offset * scale;
            }

            if (restAlignment == LayoutAlignment.End)
            {
                double startOffset = Math.Max(0, availableWidth - desiredWidth);
                double endOffset = Math.Max(0, (availableWidth - desiredWidth) / 2);
                double endOffsetDelta = startOffset - endOffset;
                return endOffset + endOffsetDelta * (1 - scale);
            }

            return Math.Max(0, (availableWidth - desiredWidth * scaleMultiplier) / 2);
        }

        double FloatRight()
        {
            if (restAlignment == LayoutAlignment.Start || restAlignment == LayoutAlignment.Fill)
            {
                double offset = Math.Max(0, availableWidth - desiredWidth * scaleMultiplier);
                return offset * scale;
            }

            if (restAlignment == LayoutAlignment.Center)
            {
                double startOffset = Math.Max(0, (availableWidth - desiredWidth) / 2);
                double endOffsetDelta = Math.Max(0, availableWidth - desiredWidth * scaleMultiplier) - startOffset;
                return startOffset + endOffsetDelta * scale;
            }

            return Math.Max(0, availableWidth - desiredWidth * scaleMultiplier);
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

    private static bool TryGetAlignment(object? value, out LayoutAlignment alignment)
    {
        if (value is LayoutAlignment layoutAlignment)
        {
            alignment = layoutAlignment;
            return true;
        }

        if (value is LayoutOptions layoutOptions)
        {
            alignment = layoutOptions.Alignment;
            return true;
        }

        if (value is string text && TryParseAlignment(text, out alignment))
        {
            return true;
        }

        if (value is Enum enumValue && TryParseAlignment(enumValue.ToString(), out alignment))
        {
            return true;
        }

        alignment = LayoutAlignment.Start;
        return false;
    }

    private static AlignmentOverride ParseAlignmentOverride(object? value)
    {
        if (value is null)
        {
            return AlignmentOverride.Inherit;
        }

        if (value is string text)
        {
            return ParseAlignmentOverride(text);
        }

        if (value is Enum enumValue)
        {
            return ParseAlignmentOverride(enumValue.ToString());
        }

        if (value is LayoutAlignment alignment)
        {
            return AlignmentOverrideExtensions.FromAlignment(alignment);
        }

        if (value is LayoutOptions layoutOptions)
        {
            return AlignmentOverrideExtensions.FromAlignment(layoutOptions.Alignment);
        }

        return AlignmentOverride.Inherit;
    }

    private static AlignmentOverride ParseAlignmentOverride(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return AlignmentOverride.Inherit;
        }

        if (string.Equals(text, "Inherit", StringComparison.OrdinalIgnoreCase))
        {
            return AlignmentOverride.Inherit;
        }

        if (TryParseAlignment(text, out var alignment))
        {
            return AlignmentOverrideExtensions.FromAlignment(alignment);
        }

        if (string.Equals(text, "Stretch", StringComparison.OrdinalIgnoreCase))
        {
            return AlignmentOverrideExtensions.FromAlignment(LayoutAlignment.Fill);
        }

        return AlignmentOverride.Inherit;
    }

    private static LayoutAlignment ResolveAlignment(AlignmentOverride overrideValue, LayoutAlignment fallback)
    {
        return overrideValue switch
        {
            AlignmentOverride.Inherit => fallback,
            AlignmentOverride.Start => LayoutAlignment.Start,
            AlignmentOverride.Center => LayoutAlignment.Center,
            AlignmentOverride.End => LayoutAlignment.End,
            AlignmentOverride.Fill => LayoutAlignment.Fill,
            _ => fallback
        };
    }

    private static bool TryParseAlignment(string text, out LayoutAlignment alignment)
    {
        if (Enum.TryParse(text, true, out LayoutAlignment parsed))
        {
            alignment = parsed;
            return true;
        }

        if (string.Equals(text, "Left", StringComparison.OrdinalIgnoreCase))
        {
            alignment = LayoutAlignment.Start;
            return true;
        }

        if (string.Equals(text, "Right", StringComparison.OrdinalIgnoreCase))
        {
            alignment = LayoutAlignment.End;
            return true;
        }

        if (string.Equals(text, "Stretch", StringComparison.OrdinalIgnoreCase))
        {
            alignment = LayoutAlignment.Fill;
            return true;
        }

        alignment = LayoutAlignment.Start;
        return false;
    }

    private enum AlignmentOverride
    {
        Inherit,
        Start,
        Center,
        End,
        Fill
    }

    private static class AlignmentOverrideExtensions
    {
        public static AlignmentOverride FromAlignment(LayoutAlignment alignment)
        {
            return alignment switch
            {
                LayoutAlignment.Start => AlignmentOverride.Start,
                LayoutAlignment.Center => AlignmentOverride.Center,
                LayoutAlignment.End => AlignmentOverride.End,
                LayoutAlignment.Fill => AlignmentOverride.Fill,
                _ => AlignmentOverride.Inherit
            };
        }
    }
}
