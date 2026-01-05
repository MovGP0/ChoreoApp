namespace MaterialDesignThemes.Maui;

public static class MenuItemAssist
{
    public static readonly BindableProperty HighlightedBackgroundProperty =
        BindableProperty.CreateAttached(
            "HighlightedBackground",
            typeof(Brush),
            typeof(MenuItemAssist),
            null);

    public static Brush? GetHighlightedBackground(BindableObject obj) =>
        (Brush?)obj.GetValue(HighlightedBackgroundProperty);

    public static void SetHighlightedBackground(BindableObject obj, Brush? value) =>
        obj.SetValue(HighlightedBackgroundProperty, value);
}
