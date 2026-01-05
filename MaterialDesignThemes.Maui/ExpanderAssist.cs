namespace MaterialDesignThemes.Maui;

public enum ExpanderButtonPosition
{
    Default,
    Start,
    End
}

public static class ExpanderAssist
{
    private static readonly Thickness DefaultHorizontalHeaderPadding = new(24, 12, 24, 12);
    private static readonly Thickness DefaultVerticalHeaderPadding = new(12, 24, 12, 24);

    public static readonly BindableProperty HorizontalHeaderPaddingProperty =
        BindableProperty.CreateAttached(
            "HorizontalHeaderPadding",
            typeof(Thickness),
            typeof(ExpanderAssist),
            DefaultHorizontalHeaderPadding);

    public static Thickness GetHorizontalHeaderPadding(BindableObject element) =>
        (Thickness)element.GetValue(HorizontalHeaderPaddingProperty);

    public static void SetHorizontalHeaderPadding(BindableObject element, Thickness value) =>
        element.SetValue(HorizontalHeaderPaddingProperty, value);

    public static readonly BindableProperty VerticalHeaderPaddingProperty =
        BindableProperty.CreateAttached(
            "VerticalHeaderPadding",
            typeof(Thickness),
            typeof(ExpanderAssist),
            DefaultVerticalHeaderPadding);

    public static Thickness GetVerticalHeaderPadding(BindableObject element) =>
        (Thickness)element.GetValue(VerticalHeaderPaddingProperty);

    public static void SetVerticalHeaderPadding(BindableObject element, Thickness value) =>
        element.SetValue(VerticalHeaderPaddingProperty, value);

    public static readonly BindableProperty HeaderFontSizeProperty =
        BindableProperty.CreateAttached(
            "HeaderFontSize",
            typeof(double),
            typeof(ExpanderAssist),
            15.0);

    public static double GetHeaderFontSize(BindableObject element) =>
        (double)element.GetValue(HeaderFontSizeProperty);

    public static void SetHeaderFontSize(BindableObject element, double value) =>
        element.SetValue(HeaderFontSizeProperty, value);

    public static readonly BindableProperty HeaderBackgroundProperty =
        BindableProperty.CreateAttached(
            "HeaderBackground",
            typeof(Brush),
            typeof(ExpanderAssist),
            default(Brush));

    public static Brush? GetHeaderBackground(BindableObject element) =>
        (Brush?)element.GetValue(HeaderBackgroundProperty);

    public static void SetHeaderBackground(BindableObject element, Brush? value) =>
        element.SetValue(HeaderBackgroundProperty, value);

    public static readonly BindableProperty ExpanderButtonContentProperty =
        BindableProperty.CreateAttached(
            "ExpanderButtonContent",
            typeof(object),
            typeof(ExpanderAssist),
            default(object));

    public static object? GetExpanderButtonContent(BindableObject element) =>
        element.GetValue(ExpanderButtonContentProperty);

    public static void SetExpanderButtonContent(BindableObject element, object? value) =>
        element.SetValue(ExpanderButtonContentProperty, value);

    public static readonly BindableProperty ExpanderButtonPositionProperty =
        BindableProperty.CreateAttached(
            "ExpanderButtonPosition",
            typeof(ExpanderButtonPosition),
            typeof(ExpanderAssist),
            ExpanderButtonPosition.Default);

    public static ExpanderButtonPosition GetExpanderButtonPosition(BindableObject element) =>
        (ExpanderButtonPosition)element.GetValue(ExpanderButtonPositionProperty);

    public static void SetExpanderButtonPosition(BindableObject element, ExpanderButtonPosition value) =>
        element.SetValue(ExpanderButtonPositionProperty, value);
}
