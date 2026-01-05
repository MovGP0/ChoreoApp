namespace MaterialDesignThemes.Maui;

public static class SliderAssist
{
    public static readonly BindableProperty HideActiveTrackProperty = BindableProperty.CreateAttached(
        "HideActiveTrack",
        typeof(bool),
        typeof(SliderAssist),
        false);

    public static bool GetHideActiveTrack(BindableObject element) =>
        (bool)element.GetValue(HideActiveTrackProperty);

    public static void SetHideActiveTrack(BindableObject element, bool value) =>
        element.SetValue(HideActiveTrackProperty, value);

    public static readonly BindableProperty OnlyShowFocusVisualWhileDraggingProperty = BindableProperty.CreateAttached(
        "OnlyShowFocusVisualWhileDragging",
        typeof(bool),
        typeof(SliderAssist),
        false);

    public static bool GetOnlyShowFocusVisualWhileDragging(BindableObject element) =>
        (bool)element.GetValue(OnlyShowFocusVisualWhileDraggingProperty);

    public static void SetOnlyShowFocusVisualWhileDragging(BindableObject element, bool value) =>
        element.SetValue(OnlyShowFocusVisualWhileDraggingProperty, value);

    public static readonly BindableProperty ToolTipFormatProperty = BindableProperty.CreateAttached(
        "ToolTipFormat",
        typeof(string),
        typeof(SliderAssist),
        null);

    public static string? GetToolTipFormat(BindableObject element) =>
        (string?)element.GetValue(ToolTipFormatProperty);

    public static void SetToolTipFormat(BindableObject element, string? value) =>
        element.SetValue(ToolTipFormatProperty, value);

    public static readonly BindableProperty FocusSliderOnClickProperty = BindableProperty.CreateAttached(
        "FocusSliderOnClick",
        typeof(bool),
        typeof(SliderAssist),
        false,
        propertyChanged: OnFocusSliderOnClickChanged);

    public static bool GetFocusSliderOnClick(BindableObject element) =>
        (bool)element.GetValue(FocusSliderOnClickProperty);

    public static void SetFocusSliderOnClick(BindableObject element, bool value) =>
        element.SetValue(FocusSliderOnClickProperty, value);

    private static void OnFocusSliderOnClickChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Slider slider)
        {
            return;
        }

        if ((bool)newValue)
        {
            slider.DragStarted += OnSliderDragStarted;
        }
        else
        {
            slider.DragStarted -= OnSliderDragStarted;
        }
    }

    private static void OnSliderDragStarted(object? sender, EventArgs e)
    {
        if (sender is Slider slider)
        {
            slider.Focus();
        }
    }
}
