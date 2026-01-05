namespace MaterialDesignThemes.Maui;

public static class ListBoxItemAssist
{
    private static readonly CornerRadius DefaultCornerRadius = new(2.0);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.CreateAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(ListBoxItemAssist),
            DefaultCornerRadius);

    public static CornerRadius GetCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(CornerRadiusProperty);

    public static void SetCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(CornerRadiusProperty, value);

    public static readonly BindableProperty HoverBackgroundProperty =
        BindableProperty.CreateAttached(
            "HoverBackground",
            typeof(Brush),
            typeof(ListBoxItemAssist),
            default(Brush));

    public static Brush? GetHoverBackground(BindableObject obj) =>
        (Brush?)obj.GetValue(HoverBackgroundProperty);

    public static void SetHoverBackground(BindableObject obj, Brush? value) =>
        obj.SetValue(HoverBackgroundProperty, value);

    public static readonly BindableProperty SelectedFocusedBackgroundProperty =
        BindableProperty.CreateAttached(
            "SelectedFocusedBackground",
            typeof(Brush),
            typeof(ListBoxItemAssist),
            default(Brush));

    public static Brush? GetSelectedFocusedBackground(BindableObject obj) =>
        (Brush?)obj.GetValue(SelectedFocusedBackgroundProperty);

    public static void SetSelectedFocusedBackground(BindableObject obj, Brush? value) =>
        obj.SetValue(SelectedFocusedBackgroundProperty, value);

    public static readonly BindableProperty SelectedUnfocusedBackgroundProperty =
        BindableProperty.CreateAttached(
            "SelectedUnfocusedBackground",
            typeof(Brush),
            typeof(ListBoxItemAssist),
            default(Brush));

    public static Brush? GetSelectedUnfocusedBackground(BindableObject obj) =>
        (Brush?)obj.GetValue(SelectedUnfocusedBackgroundProperty);

    public static void SetSelectedUnfocusedBackground(BindableObject obj, Brush? value) =>
        obj.SetValue(SelectedUnfocusedBackgroundProperty, value);

    public static readonly BindableProperty ShowSelectionProperty =
        BindableProperty.CreateAttached(
            "ShowSelection",
            typeof(bool),
            typeof(ListBoxItemAssist),
            true);

    public static bool GetShowSelection(BindableObject element) =>
        (bool)element.GetValue(ShowSelectionProperty);

    public static void SetShowSelection(BindableObject element, bool value) =>
        element.SetValue(ShowSelectionProperty, value);

    public static readonly BindableProperty CursorProperty =
        BindableProperty.CreateAttached(
            "Cursor",
            typeof(CursorIcon),
            typeof(ListBoxItemAssist),
            CursorIcon.Hand);

    public static CursorIcon GetCursor(BindableObject obj) =>
        (CursorIcon)obj.GetValue(CursorProperty);

    public static void SetCursor(BindableObject obj, CursorIcon value) =>
        obj.SetValue(CursorProperty, value);
}
