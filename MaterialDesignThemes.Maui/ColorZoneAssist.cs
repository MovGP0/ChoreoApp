namespace MaterialDesignThemes.Maui;

public static class ColorZoneAssist
{
    public static readonly BindableProperty ModeProperty =
        BindableProperty.CreateAttached(
            "Mode",
            typeof(ColorZoneMode),
            typeof(ColorZoneAssist),
            default(ColorZoneMode));

    public static void SetMode(BindableObject element, ColorZoneMode value) =>
        element.SetValue(ModeProperty, value);

    public static ColorZoneMode GetMode(BindableObject element) =>
        (ColorZoneMode)element.GetValue(ModeProperty);

    public static readonly BindableProperty BackgroundProperty =
        BindableProperty.CreateAttached(
            "Background",
            typeof(Brush),
            typeof(ColorZoneAssist),
            null);

    public static void SetBackground(BindableObject element, Brush value) =>
        element.SetValue(BackgroundProperty, value);

    public static Brush? GetBackground(BindableObject element) =>
        (Brush?)element.GetValue(BackgroundProperty);

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.CreateAttached(
            "Foreground",
            typeof(Brush),
            typeof(ColorZoneAssist),
            null);

    public static void SetForeground(BindableObject element, Brush value) =>
        element.SetValue(ForegroundProperty, value);

    public static Brush? GetForeground(BindableObject element) =>
        (Brush?)element.GetValue(ForegroundProperty);
}
