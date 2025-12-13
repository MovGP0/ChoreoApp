using System.Globalization;
using ChoreoApp.Styling;

namespace ChoreoApp.AudioPlayer.Converters;

public sealed class PlayPauseGlyphConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isPlaying)
        {
            return isPlaying ? PackIconKind.Pause : PackIconKind.Play;
        }

        return PackIconKind.Play;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
