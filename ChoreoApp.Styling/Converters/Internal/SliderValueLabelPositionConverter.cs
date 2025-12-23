using System.Globalization;

namespace ChoreoApp.Styling.Converters.Internal;

public sealed class SliderValueLabelPositionConverter : IValueConverter
{
    public static readonly SliderValueLabelPositionConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double width || !TryGetOrientation(parameter, out var orientation))
        {
            return 0d;
        }

        const double halfGripWidth = 9.0;
        const double margin = 4.0;

        return orientation == OrientationKind.Horizontal
            ? -width * 0.5 + halfGripWidth
            : -width - margin;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static bool TryGetOrientation(object? value, out OrientationKind orientation)
    {
        switch (value)
        {
            case null:
                orientation = OrientationKind.Horizontal;
                return false;
            case StackOrientation stackOrientation:
                orientation = stackOrientation == StackOrientation.Vertical
                    ? OrientationKind.Vertical
                    : OrientationKind.Horizontal;
                return true;
            case ItemsLayoutOrientation itemsLayoutOrientation:
                orientation = itemsLayoutOrientation == ItemsLayoutOrientation.Vertical
                    ? OrientationKind.Vertical
                    : OrientationKind.Horizontal;
                return true;
            case string text when string.Equals(text, "Vertical", StringComparison.OrdinalIgnoreCase):
                orientation = OrientationKind.Vertical;
                return true;
            case string text when string.Equals(text, "Horizontal", StringComparison.OrdinalIgnoreCase):
                orientation = OrientationKind.Horizontal;
                return true;
            case Enum enumValue:
                return TryGetOrientation(enumValue.ToString(), out orientation);
            default:
                orientation = OrientationKind.Horizontal;
                return false;
        }
    }

    private enum OrientationKind
    {
        Horizontal,
        Vertical
    }
}
