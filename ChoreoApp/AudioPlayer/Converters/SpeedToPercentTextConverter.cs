using System.Globalization;

namespace ChoreoApp.AudioPlayer.Converters;

public sealed class SpeedToPercentTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double speed || double.IsNaN(speed) || double.IsInfinity(speed))
        {
            return "0%";
        }

        var percent = (int)Math.Round(speed * 100d, MidpointRounding.AwayFromZero);
        return $"{percent}%";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
