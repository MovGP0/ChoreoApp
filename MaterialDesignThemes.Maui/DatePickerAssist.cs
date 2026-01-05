namespace MaterialDesignThemes.Maui;

public static class DatePickerAssist
{
    private const string AnimateBackgroundName = "MaterialDesignDatePickerBackground";
    public static readonly BindableProperty OutlinedBorderInactiveThicknessProperty =
        BindableProperty.CreateAttached(
            "OutlinedBorderInactiveThickness",
            typeof(Thickness),
            typeof(DatePickerAssist),
            Constants.DefaultOutlinedBorderInactiveThickness);

    public static void SetOutlinedBorderInactiveThickness(BindableObject element, Thickness value) =>
        element.SetValue(OutlinedBorderInactiveThicknessProperty, value);

    public static Thickness GetOutlinedBorderInactiveThickness(BindableObject element) =>
        (Thickness)element.GetValue(OutlinedBorderInactiveThicknessProperty);

    public static readonly BindableProperty OutlinedBorderActiveThicknessProperty =
        BindableProperty.CreateAttached(
            "OutlinedBorderActiveThickness",
            typeof(Thickness),
            typeof(DatePickerAssist),
            Constants.DefaultOutlinedBorderActiveThickness);

    public static void SetOutlinedBorderActiveThickness(BindableObject element, Thickness value) =>
        element.SetValue(OutlinedBorderActiveThicknessProperty, value);

    public static Thickness GetOutlinedBorderActiveThickness(BindableObject element) =>
        (Thickness)element.GetValue(OutlinedBorderActiveThicknessProperty);

    public static readonly BindableProperty AnimateOnFocusProperty =
        BindableProperty.CreateAttached(
            "AnimateOnFocus",
            typeof(bool),
            typeof(DatePickerAssist),
            false,
            propertyChanged: OnAnimateOnFocusChanged);

    public static bool GetAnimateOnFocus(BindableObject element) =>
        (bool)element.GetValue(AnimateOnFocusProperty);

    public static void SetAnimateOnFocus(BindableObject element, bool value) =>
        element.SetValue(AnimateOnFocusProperty, value);

    public static readonly BindableProperty FocusedBackgroundColorProperty =
        BindableProperty.CreateAttached(
            "FocusedBackgroundColor",
            typeof(Color),
            typeof(DatePickerAssist),
            Colors.Transparent);

    public static Color GetFocusedBackgroundColor(BindableObject element) =>
        (Color)element.GetValue(FocusedBackgroundColorProperty);

    public static void SetFocusedBackgroundColor(BindableObject element, Color value) =>
        element.SetValue(FocusedBackgroundColorProperty, value);

    public static readonly BindableProperty UnfocusedBackgroundColorProperty =
        BindableProperty.CreateAttached(
            "UnfocusedBackgroundColor",
            typeof(Color),
            typeof(DatePickerAssist),
            Colors.Transparent);

    public static Color GetUnfocusedBackgroundColor(BindableObject element) =>
        (Color)element.GetValue(UnfocusedBackgroundColorProperty);

    public static void SetUnfocusedBackgroundColor(BindableObject element, Color value) =>
        element.SetValue(UnfocusedBackgroundColorProperty, value);

    private static void OnAnimateOnFocusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not DatePicker datePicker)
        {
            return;
        }

        if ((bool)newValue)
        {
            datePicker.Focused -= OnFocused;
            datePicker.Unfocused -= OnUnfocused;
            datePicker.Focused += OnFocused;
            datePicker.Unfocused += OnUnfocused;
        }
        else
        {
            datePicker.Focused -= OnFocused;
            datePicker.Unfocused -= OnUnfocused;
        }
    }

    private static void OnFocused(object? sender, FocusEventArgs e)
    {
        if (sender is not DatePicker datePicker)
        {
            return;
        }

        AnimateBackground(datePicker, GetFocusedBackgroundColor(datePicker));
    }

    private static void OnUnfocused(object? sender, FocusEventArgs e)
    {
        if (sender is not DatePicker datePicker)
        {
            return;
        }

        AnimateBackground(datePicker, GetUnfocusedBackgroundColor(datePicker));
    }

    private static void AnimateBackground(VisualElement element, Color target)
    {
        const uint duration = 160;
        var start = element.BackgroundColor;

        element.AbortAnimation(AnimateBackgroundName);
        var animation = new Animation(
            callback: value => element.BackgroundColor = Color.FromRgba(
                start.Red + (target.Red - start.Red) * value,
                start.Green + (target.Green - start.Green) * value,
                start.Blue + (target.Blue - start.Blue) * value,
                start.Alpha + (target.Alpha - start.Alpha) * value),
            start: 0,
            end: 1,
            easing: Easing.CubicOut);

        animation.Commit(element, AnimateBackgroundName, 16, duration);
    }
}
