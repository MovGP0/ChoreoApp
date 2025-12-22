using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class TextFieldPrefixTextVisibilityConverter : IMultiValueConverter
{
    public bool HiddenState { get; set; }

    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not
            [
                bool isHintInFloatingPosition,
                string prefixText,
                var prefixSuffixVisibility,
                bool isKeyboardFocusWithin,
                bool isEditable
            ])
        {
            return false;
        }

        if (string.IsNullOrEmpty(prefixText))
        {
            return false;
        }

        if (IsVisibilityAlways(prefixSuffixVisibility))
        {
            return true;
        }

        return isHintInFloatingPosition || isKeyboardFocusWithin || !isEditable ? true : HiddenState;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
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
