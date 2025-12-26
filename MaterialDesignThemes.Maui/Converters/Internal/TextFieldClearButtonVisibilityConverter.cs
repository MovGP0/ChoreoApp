using System.Globalization;

namespace MaterialDesignThemes.Maui.Converters.Internal;

public sealed class TextFieldClearButtonVisibilityConverter : IMultiValueConverter
{
    public bool ContentEmptyVisibility { get; set; }

    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [bool hasClearButton, bool isContentNullOrEmpty, ..])
        {
            return true;
        }

        if (!hasClearButton)
        {
            return false;
        }

        if (isContentNullOrEmpty && values.Length > 2 && values[2] is bool isEditable && !isEditable)
        {
            return false;
        }

        return !isContentNullOrEmpty || ContentEmptyVisibility;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
