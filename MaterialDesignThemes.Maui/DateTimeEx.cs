using System.Globalization;

namespace MaterialDesignThemes.Maui;

internal static class DateTimeEx
{
    internal static DateTimeFormatInfo GetDateFormat(this CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(culture);

        if (culture.Calendar is GregorianCalendar or PersianCalendar)
        {
            return culture.DateTimeFormat;
        }

        GregorianCalendar? foundCal = null;
        foreach (var cal in culture.OptionalCalendars.OfType<GregorianCalendar>())
        {
            foundCal ??= cal;

            if (cal.CalendarType != GregorianCalendarTypes.Localized)
            {
                continue;
            }

            foundCal = cal;
            break;
        }

        DateTimeFormatInfo dtfi;
        if (foundCal is null)
        {
            dtfi = ((CultureInfo)CultureInfo.InvariantCulture.Clone()).DateTimeFormat;
            dtfi.Calendar = new GregorianCalendar();
        }
        else
        {
            dtfi = ((CultureInfo)culture.Clone()).DateTimeFormat;
            dtfi.Calendar = foundCal;
        }

        return dtfi;
    }
}
