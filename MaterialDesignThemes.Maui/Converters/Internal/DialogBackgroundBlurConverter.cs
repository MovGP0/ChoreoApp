using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class DialogBackgroundBlurConverter : IMultiValueConverter
{
    public static readonly DialogBackgroundBlurConverter Instance = new();

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (TryGetInput(values, out var visualElement, out var isOpen, out var applyBlurBackground, out var blurRadius))
        {
            bool isEnabled = isOpen && applyBlurBackground;

            if (visualElement is not null)
            {
                visualElement.SetDialogBackgroundBlur(isEnabled, blurRadius);
                return BindableProperty.UnsetValue;
            }

            return ToTarget(isEnabled ? blurRadius : 0d, targetType);
        }

        return ToTarget(0d, targetType);
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static object? ToTarget(double value, Type targetType)
    {
        if (targetType == typeof(float))
        {
            return (float)value;
        }

        if (targetType == typeof(double) || targetType == typeof(object))
        {
            return value;
        }

        if (targetType == typeof(int))
        {
            return (int)Math.Round(value);
        }

        return null;
    }

    private static bool TryGetInput(
        object?[]? values,
        out VisualElement? visualElement,
        out bool isOpen,
        out bool applyBlurBackground,
        out double blurRadius)
    {
        visualElement = null;
        isOpen = false;
        applyBlurBackground = false;
        blurRadius = 0d;

        if (values is null || values.Length == 0)
        {
            return false;
        }

        foreach (var value in values)
        {
            switch (value)
            {
                case VisualElement element:
                    visualElement = element;
                    break;
                case bool flag when !isOpen:
                    isOpen = flag;
                    break;
                case bool flag:
                    applyBlurBackground = flag;
                    break;
                case double d:
                    blurRadius = d;
                    break;
                case float f:
                    blurRadius = f;
                    break;
                case int i:
                    blurRadius = i;
                    break;
            }
        }

        return true;
    }
}
