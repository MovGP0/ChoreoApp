namespace MaterialDesignThemes.Maui;

public static class ButtonAssist
{
    private static readonly CornerRadius DefaultCornerRadius = new(2.0);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.CreateAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(ButtonAssist),
            DefaultCornerRadius);

    public static CornerRadius GetCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(CornerRadiusProperty);

    public static void SetCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(CornerRadiusProperty, value);
}
