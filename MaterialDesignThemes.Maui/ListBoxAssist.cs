namespace MaterialDesignThemes.Maui;

public static class ListBoxAssist
{
    public static readonly BindableProperty IsToggleProperty =
        BindableProperty.CreateAttached(
            "IsToggle",
            typeof(bool),
            typeof(ListBoxAssist),
            false);

    public static void SetIsToggle(BindableObject element, bool value) =>
        element.SetValue(IsToggleProperty, value);

    public static bool GetIsToggle(BindableObject element) =>
        (bool)element.GetValue(IsToggleProperty);

    public static readonly BindableProperty CanUserToggleSelectedItemProperty =
        BindableProperty.CreateAttached(
            "CanUserToggleSelectedItem",
            typeof(bool),
            typeof(ListBoxAssist),
            false);

    public static void SetCanUserToggleSelectedItem(BindableObject element, bool value) =>
        element.SetValue(CanUserToggleSelectedItemProperty, value);

    public static bool GetCanUserToggleSelectedItem(BindableObject element) =>
        (bool)element.GetValue(CanUserToggleSelectedItemProperty);
}
