namespace MaterialDesignThemes.Maui;

public enum CalendarOrientation
{
    Vertical,
    Horizontal
}

public static class CalendarAssist
{
    public static readonly BindableProperty IsHeaderVisibleProperty = BindableProperty.CreateAttached(
        "IsHeaderVisible",
        typeof(bool),
        typeof(CalendarAssist),
        true);

    public static bool GetIsHeaderVisible(BindableObject element) =>
        (bool)element.GetValue(IsHeaderVisibleProperty);

    public static void SetIsHeaderVisible(BindableObject element, bool value) =>
        element.SetValue(IsHeaderVisibleProperty, value);

    public static readonly BindableProperty HeaderBackgroundProperty = BindableProperty.CreateAttached(
        "HeaderBackground",
        typeof(Brush),
        typeof(CalendarAssist),
        null);

    public static Brush? GetHeaderBackground(BindableObject element) =>
        (Brush?)element.GetValue(HeaderBackgroundProperty);

    public static void SetHeaderBackground(BindableObject element, Brush? value) =>
        element.SetValue(HeaderBackgroundProperty, value);

    public static readonly BindableProperty HeaderForegroundProperty = BindableProperty.CreateAttached(
        "HeaderForeground",
        typeof(Brush),
        typeof(CalendarAssist),
        null);

    public static Brush? GetHeaderForeground(BindableObject element) =>
        (Brush?)element.GetValue(HeaderForegroundProperty);

    public static void SetHeaderForeground(BindableObject element, Brush? value) =>
        element.SetValue(HeaderForegroundProperty, value);

    public static readonly BindableProperty SelectionColorProperty = BindableProperty.CreateAttached(
        "SelectionColor",
        typeof(Brush),
        typeof(CalendarAssist),
        null);

    public static Brush? GetSelectionColor(BindableObject element) =>
        (Brush?)element.GetValue(SelectionColorProperty);

    public static void SetSelectionColor(BindableObject element, Brush? value) =>
        element.SetValue(SelectionColorProperty, value);

    public static readonly BindableProperty SelectionForegroundColorProperty = BindableProperty.CreateAttached(
        "SelectionForegroundColor",
        typeof(Brush),
        typeof(CalendarAssist),
        null);

    public static Brush? GetSelectionForegroundColor(BindableObject element) =>
        (Brush?)element.GetValue(SelectionForegroundColorProperty);

    public static void SetSelectionForegroundColor(BindableObject element, Brush? value) =>
        element.SetValue(SelectionForegroundColorProperty, value);

    public static readonly BindableProperty OrientationProperty = BindableProperty.CreateAttached(
        "Orientation",
        typeof(CalendarOrientation),
        typeof(CalendarAssist),
        CalendarOrientation.Vertical);

    public static CalendarOrientation GetOrientation(BindableObject element) =>
        (CalendarOrientation)element.GetValue(OrientationProperty);

    public static void SetOrientation(BindableObject element, CalendarOrientation value) =>
        element.SetValue(OrientationProperty, value);
}
