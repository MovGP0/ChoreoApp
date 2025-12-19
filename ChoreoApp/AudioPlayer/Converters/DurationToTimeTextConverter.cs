using System.Globalization;

namespace ChoreoApp.AudioPlayer.Converters;

public sealed class DurationToTimeTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double seconds || double.IsNaN(seconds) || double.IsInfinity(seconds) || seconds < 0d)
        {
            return "0:00";
        }

        var totalSeconds = (int)Math.Round(seconds, MidpointRounding.AwayFromZero);
        var time = TimeSpan.FromSeconds(totalSeconds);

        if (time.TotalHours >= 1)
        {
            return time.ToString("h\\:mm\\:ss", CultureInfo.InvariantCulture);
        }

        return time.ToString("m\\:ss", CultureInfo.InvariantCulture);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
