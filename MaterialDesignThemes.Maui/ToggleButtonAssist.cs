namespace MaterialDesignThemes.Maui;

public static class ToggleButtonAssist
{
    private static readonly BindablePropertyKey HasOnContentPropertyKey = BindableProperty.CreateAttachedReadOnly(
        "HasOnContent",
        typeof(bool),
        typeof(ToggleButtonAssist),
        false);

    public static readonly BindableProperty HasOnContentProperty = HasOnContentPropertyKey.BindableProperty;

    private static void SetHasOnContent(BindableObject element, bool value)
        => element.SetValue(HasOnContentPropertyKey, value);

    public static bool GetHasOnContent(BindableObject element)
        => (bool)element.GetValue(HasOnContentProperty);

    public static readonly BindableProperty OnContentProperty = BindableProperty.CreateAttached(
        "OnContent",
        typeof(object),
        typeof(ToggleButtonAssist),
        null,
        propertyChanged: OnContentPropertyChangedCallback);

    private static void OnContentPropertyChangedCallback(BindableObject bindable, object oldValue, object? newValue)
        => SetHasOnContent(bindable, newValue is not null);

    public static void SetOnContent(BindableObject element, object? value)
        => element.SetValue(OnContentProperty, value);

    public static object? GetOnContent(BindableObject element)
        => element.GetValue(OnContentProperty);

    public static readonly BindableProperty OnContentTemplateProperty = BindableProperty.CreateAttached(
        "OnContentTemplate",
        typeof(DataTemplate),
        typeof(ToggleButtonAssist),
        null);

    public static void SetOnContentTemplate(BindableObject element, DataTemplate? value)
        => element.SetValue(OnContentTemplateProperty, value);

    public static DataTemplate? GetOnContentTemplate(BindableObject element)
        => (DataTemplate?)element.GetValue(OnContentTemplateProperty);

    public static readonly BindableProperty SwitchTrackOnBackgroundProperty = BindableProperty.CreateAttached(
        "SwitchTrackOnBackground",
        typeof(Brush),
        typeof(ToggleButtonAssist),
        null);

    public static void SetSwitchTrackOnBackground(BindableObject element, Brush? value)
        => element.SetValue(SwitchTrackOnBackgroundProperty, value);

    public static Brush? GetSwitchTrackOnBackground(BindableObject element)
        => (Brush?)element.GetValue(SwitchTrackOnBackgroundProperty);

    public static readonly BindableProperty SwitchTrackOffBackgroundProperty = BindableProperty.CreateAttached(
        "SwitchTrackOffBackground",
        typeof(Brush),
        typeof(ToggleButtonAssist),
        null);

    public static void SetSwitchTrackOffBackground(BindableObject element, Brush? value)
        => element.SetValue(SwitchTrackOffBackgroundProperty, value);

    public static Brush? GetSwitchTrackOffBackground(BindableObject element)
        => (Brush?)element.GetValue(SwitchTrackOffBackgroundProperty);
}
