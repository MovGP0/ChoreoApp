using System.Globalization;
using System.Text;

namespace MaterialDesignThemes.Maui;

public enum DatePickerFormat
{
    Short,
    Long
}

public class TimePicker : Microsoft.Maui.Controls.TimePicker
{
    private bool _isUpdating;
    private DateTime? _lastValidTime;

    public TimePicker()
    {
        TimeSelected += OnTimeSelected;
        UpdateFormat();
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(TimePicker),
        null,
        BindingMode.TwoWay,
        propertyChanged: OnTextChanged);

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty SelectedTimeProperty = BindableProperty.Create(
        nameof(SelectedTime),
        typeof(DateTime?),
        typeof(TimePicker),
        null,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedTimeChanged);

    public DateTime? SelectedTime
    {
        get => (DateTime?)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public event EventHandler<TimeChangedEventArgs>? SelectedTimeChanged;

    public static readonly BindableProperty SelectedTimeFormatProperty = BindableProperty.Create(
        nameof(SelectedTimeFormat),
        typeof(DatePickerFormat),
        typeof(TimePicker),
        DatePickerFormat.Short,
        propertyChanged: OnFormatChanged);

    public DatePickerFormat SelectedTimeFormat
    {
        get => (DatePickerFormat)GetValue(SelectedTimeFormatProperty);
        set => SetValue(SelectedTimeFormatProperty, value);
    }

    public static readonly BindableProperty IsDropDownOpenProperty = BindableProperty.Create(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(TimePicker),
        false);

    public bool IsDropDownOpen
    {
        get => (bool)GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly BindableProperty Is24HoursProperty = BindableProperty.Create(
        nameof(Is24Hours),
        typeof(bool),
        typeof(TimePicker),
        false,
        propertyChanged: OnFormatChanged);

    public bool Is24Hours
    {
        get => (bool)GetValue(Is24HoursProperty);
        set => SetValue(Is24HoursProperty, value);
    }

    public static readonly BindableProperty IsHeaderVisibleProperty = BindableProperty.Create(
        nameof(IsHeaderVisible),
        typeof(bool),
        typeof(TimePicker),
        false);

    public bool IsHeaderVisible
    {
        get => (bool)GetValue(IsHeaderVisibleProperty);
        set => SetValue(IsHeaderVisibleProperty, value);
    }

    public static readonly BindableProperty ClockButtonVisibilityProperty = BindableProperty.Create(
        nameof(ClockButtonVisibility),
        typeof(Visibility),
        typeof(TimePicker),
        Visibility.Visible);

    public Visibility ClockButtonVisibility
    {
        get => (Visibility)GetValue(ClockButtonVisibilityProperty);
        set => SetValue(ClockButtonVisibilityProperty, value);
    }

    public static readonly BindableProperty ClockStyleProperty = BindableProperty.Create(
        nameof(ClockStyle),
        typeof(Style),
        typeof(TimePicker),
        null);

    public Style? ClockStyle
    {
        get => (Style?)GetValue(ClockStyleProperty);
        set => SetValue(ClockStyleProperty, value);
    }

    public static readonly BindableProperty ClockHostContentControlStyleProperty = BindableProperty.Create(
        nameof(ClockHostContentControlStyle),
        typeof(Style),
        typeof(TimePicker),
        null);

    public Style? ClockHostContentControlStyle
    {
        get => (Style?)GetValue(ClockHostContentControlStyleProperty);
        set => SetValue(ClockHostContentControlStyleProperty, value);
    }

    public static readonly BindableProperty IsInvalidTextAllowedProperty = BindableProperty.Create(
        nameof(IsInvalidTextAllowed),
        typeof(bool),
        typeof(TimePicker),
        false);

    public bool IsInvalidTextAllowed
    {
        get => (bool)GetValue(IsInvalidTextAllowedProperty);
        set => SetValue(IsInvalidTextAllowedProperty, value);
    }

    public static readonly BindableProperty WithSecondsProperty = BindableProperty.Create(
        nameof(WithSeconds),
        typeof(bool),
        typeof(TimePicker),
        false,
        propertyChanged: OnFormatChanged);

    public bool WithSeconds
    {
        get => (bool)GetValue(WithSecondsProperty);
        set => SetValue(WithSecondsProperty, value);
    }

    private static void OnSelectedTimeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TimePicker picker)
        {
            return;
        }

        if (picker._isUpdating)
        {
            return;
        }

        picker._isUpdating = true;

        var oldTime = (DateTime?)oldValue;
        var newTime = (DateTime?)newValue;
        picker._lastValidTime = newTime;

        picker.Time = newTime?.TimeOfDay;

        picker.Text = picker.DateTimeToString(newTime);
        picker.SelectedTimeChanged?.Invoke(picker, new TimeChangedEventArgs(oldTime ?? DateTime.MinValue, newTime ?? DateTime.MinValue));

        picker._isUpdating = false;
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TimePicker picker || picker._isUpdating)
        {
            return;
        }

        var text = newValue as string;
        if (string.IsNullOrWhiteSpace(text))
        {
            picker.SelectedTime = null;
            return;
        }

        if (picker.TryParseTime(text, out var time))
        {
            picker.SelectedTime = time;
        }
        else if (!picker.IsInvalidTextAllowed)
        {
            picker.Text = picker.DateTimeToString(picker._lastValidTime);
        }
    }

    private static void OnFormatChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TimePicker picker)
        {
            return;
        }

        picker.UpdateFormat();
        picker.Text = picker.DateTimeToString(picker.SelectedTime);
    }

    private void OnTimeSelected(object? sender, Microsoft.Maui.Controls.TimeChangedEventArgs e)
    {
        if (_isUpdating)
        {
            return;
        }

        _isUpdating = true;
        var date = SelectedTime?.Date ?? DateTime.Today;
        var newValue = e.NewTime.HasValue ? date.Add(e.NewTime.Value) : (DateTime?)null;
        var oldValue = e.OldTime.HasValue ? date.Add(e.OldTime.Value) : (DateTime?)null;
        SelectedTime = newValue;
        Text = DateTimeToString(newValue);
        SelectedTimeChanged?.Invoke(this, new TimeChangedEventArgs(oldValue ?? DateTime.MinValue, newValue ?? DateTime.MinValue));
        _lastValidTime = newValue;
        _isUpdating = false;
    }

    private void UpdateFormat()
    {
        Format = BuildFormatString();
    }

    private string BuildFormatString()
    {
        var culture = CultureInfo.CurrentCulture;
        var dtfi = culture.DateTimeFormat;
        var hourFormatChar = Is24Hours ? "H" : "h";

        var sb = new StringBuilder();
        sb.Append(hourFormatChar);
        if (SelectedTimeFormat == DatePickerFormat.Long)
        {
            sb.Append(hourFormatChar);
        }

        sb.Append(dtfi.TimeSeparator);
        sb.Append("mm");

        if (WithSeconds)
        {
            sb.Append(dtfi.TimeSeparator);
            sb.Append("ss");
        }

        if (!Is24Hours && (!string.IsNullOrEmpty(dtfi.AMDesignator) || !string.IsNullOrEmpty(dtfi.PMDesignator)))
        {
            sb.Append(" tt");
        }

        return sb.ToString();
    }

    private string? DateTimeToString(DateTime? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        return DateTimeToString(value.Value);
    }

    private string DateTimeToString(DateTime value)
    {
        return value.ToString(BuildFormatString(), CultureInfo.CurrentCulture);
    }

    private bool TryParseTime(string text, out DateTime time)
    {
        return DateTime.TryParse(
            text,
            CultureInfo.CurrentCulture,
            DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault,
            out time);
    }
}
