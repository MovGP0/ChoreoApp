namespace MaterialDesignThemes.Maui;

public static class ComboBoxAssist
{
    public static readonly BindableProperty ShowSelectedItemProperty = BindableProperty.CreateAttached(
        "ShowSelectedItem",
        typeof(bool),
        typeof(ComboBoxAssist),
        true);

    public static bool GetShowSelectedItem(BindableObject element) =>
        (bool)element.GetValue(ShowSelectedItemProperty);

    public static void SetShowSelectedItem(BindableObject element, bool value) =>
        element.SetValue(ShowSelectedItemProperty, value);

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.CreateAttached(
        "MaxLength",
        typeof(int),
        typeof(ComboBoxAssist),
        0);

    public static int GetMaxLength(BindableObject element) =>
        (int)element.GetValue(MaxLengthProperty);

    public static void SetMaxLength(BindableObject element, int value) =>
        element.SetValue(MaxLengthProperty, value);
}
