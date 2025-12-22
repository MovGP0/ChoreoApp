using System.Globalization;

namespace ChoreoApp.Styling.Converters;

public sealed class CursorConverter : IValueConverter
{
    public static readonly CursorConverter ArrowInstance = new()
    {
        FallbackCursor = CursorIcon.Arrow
    };

    public static readonly CursorConverter IBeamInstance = new()
    {
        FallbackCursor = CursorIcon.IBeam
    };

    public CursorIcon FallbackCursor { get; set; } = CursorIcon.Arrow;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is CursorIcon cursor ? cursor : FallbackCursor;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
