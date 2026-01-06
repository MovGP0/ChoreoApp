namespace MaterialDesignThemes.Maui;

public static class PickerAssist
{
    private const string AnimateBackgroundName = "MaterialDesignPickerBackground";

    public static readonly BindableProperty AnimateOnFocusProperty =
        BindableProperty.CreateAttached(
            "AnimateOnFocus",
            typeof(bool),
            typeof(PickerAssist),
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
            typeof(PickerAssist),
            Colors.Transparent);

    public static Color GetFocusedBackgroundColor(BindableObject element) =>
        (Color)element.GetValue(FocusedBackgroundColorProperty);

    public static void SetFocusedBackgroundColor(BindableObject element, Color value) =>
        element.SetValue(FocusedBackgroundColorProperty, value);

    public static readonly BindableProperty UnfocusedBackgroundColorProperty =
        BindableProperty.CreateAttached(
            "UnfocusedBackgroundColor",
            typeof(Color),
            typeof(PickerAssist),
            Colors.Transparent);

    public static Color GetUnfocusedBackgroundColor(BindableObject element) =>
        (Color)element.GetValue(UnfocusedBackgroundColorProperty);

    public static void SetUnfocusedBackgroundColor(BindableObject element, Color value) =>
        element.SetValue(UnfocusedBackgroundColorProperty, value);

    private static void OnAnimateOnFocusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Picker picker)
        {
            return;
        }

        if ((bool)newValue)
        {
            picker.Focused -= OnFocused;
            picker.Unfocused -= OnUnfocused;
            picker.Focused += OnFocused;
            picker.Unfocused += OnUnfocused;
        }
        else
        {
            picker.Focused -= OnFocused;
            picker.Unfocused -= OnUnfocused;
        }
    }

    private static void OnFocused(object? sender, FocusEventArgs e)
    {
        if (sender is not Picker picker)
        {
            return;
        }

        AnimateBackground(picker, GetFocusedBackgroundColor(picker));
    }

    private static void OnUnfocused(object? sender, FocusEventArgs e)
    {
        if (sender is not Picker picker)
        {
            return;
        }

        AnimateBackground(picker, GetUnfocusedBackgroundColor(picker));
    }

    private static void AnimateBackground(VisualElement element, Color target)
    {
        try
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
        catch
        {
            // ignore
        }
    }
}
