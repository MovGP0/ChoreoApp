namespace MaterialDesignThemes.Maui;

public static class GroupBoxAssist
{
    private static readonly Thickness DefaultHeaderPaddingThickness = new(9, 9, 9, 9);

    public static readonly BindableProperty HeaderPaddingProperty =
        BindableProperty.CreateAttached(
            "HeaderPadding",
            typeof(Thickness),
            typeof(GroupBoxAssist),
            DefaultHeaderPaddingThickness);

    public static Thickness GetHeaderPadding(BindableObject element) =>
        (Thickness)element.GetValue(HeaderPaddingProperty);

    public static void SetHeaderPadding(BindableObject element, Thickness headerPadding) =>
        element.SetValue(HeaderPaddingProperty, headerPadding);
}
