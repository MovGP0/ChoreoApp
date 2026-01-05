using System.Globalization;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Provides culture-specific information about the format of calendar.
/// </summary>
public sealed class CalendarFormatInfo
{
    public string YearMonthPattern { get; }

    public string ComponentOnePattern { get; }

    public string ComponentTwoPattern { get; }

    public string ComponentThreePattern { get; }

    private const string ShortDayOfWeek = "ddd";
    private const string LongDayOfWeek = "dddd";

    private static readonly Dictionary<string, CalendarFormatInfo> _formatInfoCache = new();
    private static readonly Dictionary<string, string> _cultureYearPatterns = new();
    private static readonly Dictionary<string, DayOfWeekStyle> _cultureDayOfWeekStyles = new();

    private static readonly string[] JapaneseCultureNames = ["ja", "ja-JP"];
    private static readonly string[] ZhongwenCultureNames = ["zh", "zh-CN", "zh-Hans", "zh-Hans-HK", "zh-Hans-MO", "zh-Hant", "zh-HK", "zh-MO", "zh-SG", "zh-TW"];
    private static readonly string[] KoreanCultureNames = ["ko", "ko-KR", "ko-KP"];

    private const string CjkYearSuffix = "\u5e74";
    private const string KoreanYearSuffix = "\ub144";

    static CalendarFormatInfo()
    {
        SetYearPattern(JapaneseCultureNames, "yyyy" + CjkYearSuffix);
        SetYearPattern(ZhongwenCultureNames, "yyyy" + CjkYearSuffix);
        SetYearPattern(KoreanCultureNames, "yyyy" + KoreanYearSuffix);

        var dayOfWeekStyle = new DayOfWeekStyle(LongDayOfWeek, string.Empty, false);
        SetDayOfWeekStyle(JapaneseCultureNames, dayOfWeekStyle);
        SetDayOfWeekStyle(ZhongwenCultureNames, dayOfWeekStyle);
    }

    public static void SetYearPattern(string[] cultureNames, string yearPattern)
    {
        if (cultureNames is null)
        {
            throw new ArgumentNullException(nameof(cultureNames));
        }

        foreach (var cultureName in cultureNames)
        {
            SetYearPattern(cultureName, yearPattern);
        }
    }

    public static void SetYearPattern(string cultureName, string? yearPattern)
    {
        if (cultureName is null)
        {
            throw new ArgumentNullException(nameof(cultureName));
        }

        if (yearPattern is not null)
        {
            _cultureYearPatterns[cultureName] = yearPattern;
        }
        else
        {
            _cultureYearPatterns.Remove(cultureName);
        }

        DiscardFormatInfoCache(cultureName);
    }

    public static void SetDayOfWeekStyle(string[] cultureNames, DayOfWeekStyle dayOfWeekStyle)
    {
        if (cultureNames is null)
        {
            throw new ArgumentNullException(nameof(cultureNames));
        }

        foreach (var cultureName in cultureNames)
        {
            SetDayOfWeekStyle(cultureName, dayOfWeekStyle);
        }
    }

    public static void SetDayOfWeekStyle(string cultureName, DayOfWeekStyle dayOfWeekStyle)
    {
        if (cultureName is null)
        {
            throw new ArgumentNullException(nameof(cultureName));
        }

        _cultureDayOfWeekStyles[cultureName] = dayOfWeekStyle;
        DiscardFormatInfoCache(cultureName);
    }

    public static void ResetDayOfWeekStyle(string[] cultureNames)
    {
        if (cultureNames is null)
        {
            throw new ArgumentNullException(nameof(cultureNames));
        }

        foreach (var cultureName in cultureNames)
        {
            ResetDayOfWeekStyle(cultureName);
        }
    }

    public static void ResetDayOfWeekStyle(string cultureName)
    {
        if (cultureName is null)
        {
            throw new ArgumentNullException(nameof(cultureName));
        }

        if (_cultureDayOfWeekStyles.Remove(cultureName))
        {
            DiscardFormatInfoCache(cultureName);
        }
    }

    private static void DiscardFormatInfoCache(string cultureName) =>
        _ = _formatInfoCache.Remove(cultureName);

    private CalendarFormatInfo(string yearMonthPattern, string componentOnePattern, string componentTwoPattern, string componentThreePattern)
    {
        YearMonthPattern = yearMonthPattern;
        ComponentOnePattern = componentOnePattern;
        ComponentTwoPattern = componentTwoPattern;
        ComponentThreePattern = componentThreePattern;
    }

    public static CalendarFormatInfo FromCultureInfo(CultureInfo cultureInfo)
    {
        if (cultureInfo is null)
        {
            throw new ArgumentNullException(nameof(cultureInfo));
        }

        if (_formatInfoCache.TryGetValue(cultureInfo.Name, out var calendarInfo))
        {
            return calendarInfo;
        }

        var dateTimeFormat = cultureInfo.DateTimeFormat;

        if (!_cultureYearPatterns.TryGetValue(cultureInfo.Name, out var yearPattern))
        {
            yearPattern = "yyyy";
        }

        if (!_cultureDayOfWeekStyles.TryGetValue(cultureInfo.Name, out var dayOfWeekStyle))
        {
            dayOfWeekStyle = DayOfWeekStyle.Parse(dateTimeFormat.LongDatePattern);
        }

        var monthDayPattern = dateTimeFormat.MonthDayPattern.Replace("MMMM", "MMM");
        if (dayOfWeekStyle.IsFirst)
        {
            calendarInfo = new CalendarFormatInfo(
                dateTimeFormat.YearMonthPattern,
                monthDayPattern,
                dayOfWeekStyle.Pattern + dayOfWeekStyle.Separator,
                yearPattern);
        }
        else
        {
            calendarInfo = new CalendarFormatInfo(
                dateTimeFormat.YearMonthPattern,
                dayOfWeekStyle.Pattern,
                monthDayPattern + dayOfWeekStyle.Separator,
                yearPattern);
        }

        _formatInfoCache[cultureInfo.Name] = calendarInfo;
        return calendarInfo;
    }

    public readonly struct DayOfWeekStyle
    {
        public string Pattern { get; }

        public string Separator { get; }

        public bool IsFirst { get; }

        private const string EthiopicWordspace = "\u1361";
        private const string EthiopicComma = "\u1363";
        private const string EthiopicColon = "\u1365";
        private const string ArabicComma = "\u060c";

        private const string SeparatorChars = "," + ArabicComma + EthiopicWordspace + EthiopicComma + EthiopicColon;

        public DayOfWeekStyle(string pattern, string separator, bool isFirst)
        {
            Pattern = pattern ?? string.Empty;
            Separator = separator ?? string.Empty;
            IsFirst = isFirst;
        }

        public static DayOfWeekStyle Parse(string s)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (s.StartsWith(ShortDayOfWeek, StringComparison.Ordinal))
            {
                var index = 3;
                if (index < s.Length && s[index] == 'd')
                {
                    index++;
                }

                for (; index < s.Length && IsSpace(s[index]); index++)
                {
                }

                var separator = index < s.Length && IsSeparator(s[index]) ? s[index].ToString() : string.Empty;
                return new DayOfWeekStyle(ShortDayOfWeek, separator, true);
            }

            if (s.EndsWith(ShortDayOfWeek, StringComparison.Ordinal))
            {
                var index = s.Length - 4;
                if (index >= 0 && s[index] == 'd')
                {
                    index--;
                }

                for (; index >= 0 && IsSpace(s[index]); index--)
                {
                }

                var separator = index >= 0 && IsSeparator(s[index]) ? s[index].ToString() : string.Empty;
                return new DayOfWeekStyle(ShortDayOfWeek, separator, false);
            }

            return new DayOfWeekStyle(ShortDayOfWeek, string.Empty, true);

            static bool IsSpace(char c) => c == ' ' || c == '\'';

            static bool IsSeparator(char c) => SeparatorChars.IndexOf(c) >= 0;
        }
    }
}
