using System.Globalization;

namespace MaterialDesignThemes.Maui;

public sealed class MaterialDateDisplay : TemplatedView
{
    public MaterialDateDisplay()
    {
        DisplayDate = DateTime.Today;
        UpdateComponents();
    }

    public static readonly BindableProperty DisplayDateProperty = BindableProperty.Create(
        nameof(DisplayDate),
        typeof(DateTime),
        typeof(MaterialDateDisplay),
        default(DateTime),
        propertyChanged: OnDisplayDateChanged,
        coerceValue: CoerceDisplayDate);

    public DateTime DisplayDate
    {
        get => (DateTime)GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    private static readonly BindablePropertyKey ComponentOneContentPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(ComponentOneContent),
            typeof(string),
            typeof(MaterialDateDisplay),
            null);

    public static readonly BindableProperty ComponentOneContentProperty = ComponentOneContentPropertyKey.BindableProperty;

    public string? ComponentOneContent
    {
        get => (string?)GetValue(ComponentOneContentProperty);
        private set => SetValue(ComponentOneContentPropertyKey, value);
    }

    private static readonly BindablePropertyKey ComponentTwoContentPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(ComponentTwoContent),
            typeof(string),
            typeof(MaterialDateDisplay),
            null);

    public static readonly BindableProperty ComponentTwoContentProperty = ComponentTwoContentPropertyKey.BindableProperty;

    public string? ComponentTwoContent
    {
        get => (string?)GetValue(ComponentTwoContentProperty);
        private set => SetValue(ComponentTwoContentPropertyKey, value);
    }

    private static readonly BindablePropertyKey ComponentThreeContentPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(ComponentThreeContent),
            typeof(string),
            typeof(MaterialDateDisplay),
            null);

    public static readonly BindableProperty ComponentThreeContentProperty = ComponentThreeContentPropertyKey.BindableProperty;

    public string? ComponentThreeContent
    {
        get => (string?)GetValue(ComponentThreeContentProperty);
        private set => SetValue(ComponentThreeContentPropertyKey, value);
    }

    private static readonly BindablePropertyKey IsDayInFirstComponentPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(IsDayInFirstComponent),
            typeof(bool),
            typeof(MaterialDateDisplay),
            false);

    public static readonly BindableProperty IsDayInFirstComponentProperty =
        IsDayInFirstComponentPropertyKey.BindableProperty;

    public bool IsDayInFirstComponent
    {
        get => (bool)GetValue(IsDayInFirstComponentProperty);
        private set => SetValue(IsDayInFirstComponentPropertyKey, value);
    }

    private static object CoerceDisplayDate(BindableObject bindable, object value)
    {
        if (value is not DateTime displayDate)
        {
            return value;
        }

        var culture = CultureInfo.CurrentCulture;
        var calendar = culture.DateTimeFormat.Calendar;
        if (displayDate < calendar.MinSupportedDateTime)
        {
            return calendar.MinSupportedDateTime;
        }

        if (displayDate > calendar.MaxSupportedDateTime)
        {
            return calendar.MaxSupportedDateTime;
        }

        return displayDate;
    }

    private static void OnDisplayDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MaterialDateDisplay display)
        {
            display.UpdateComponents();
        }
    }

    private void UpdateComponents()
    {
        var culture = CultureInfo.CurrentCulture;
        var dateTimeFormatInfo = culture.DateTimeFormat;
        var minDateTime = dateTimeFormatInfo.Calendar.MinSupportedDateTime;
        var maxDateTime = dateTimeFormatInfo.Calendar.MaxSupportedDateTime;

        if (DisplayDate < minDateTime)
        {
            DisplayDate = minDateTime;
            return;
        }

        if (DisplayDate > maxDateTime)
        {
            DisplayDate = maxDateTime;
            return;
        }

        var calendarFormatInfo = CalendarFormatInfo.FromCultureInfo(culture);
        var displayDate = DisplayDate;
        ComponentOneContent = FormatDate(calendarFormatInfo.ComponentOnePattern, displayDate, culture);
        ComponentTwoContent = FormatDate(calendarFormatInfo.ComponentTwoPattern, displayDate, culture);
        ComponentThreeContent = FormatDate(calendarFormatInfo.ComponentThreePattern, displayDate, culture);
    }

    private static string FormatDate(string format, DateTime displayDate, CultureInfo culture)
    {
        return string.IsNullOrEmpty(format)
            ? string.Empty
            : displayDate.ToString(format, culture).ToTitleCase(culture);
    }
}
