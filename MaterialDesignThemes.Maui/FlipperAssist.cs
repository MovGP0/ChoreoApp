namespace MaterialDesignThemes.Maui;

public static class FlipperAssist
{
    public static readonly BindableProperty UniformCornerRadiusProperty =
        BindableProperty.CreateAttached(
            "UniformCornerRadius",
            typeof(double),
            typeof(FlipperAssist),
            0d);

    public static double GetUniformCornerRadius(BindableObject element) =>
        (double)element.GetValue(UniformCornerRadiusProperty);

    public static void SetUniformCornerRadius(BindableObject element, double value) =>
        element.SetValue(UniformCornerRadiusProperty, value);

    public static readonly BindableProperty CardStyleProperty =
        BindableProperty.CreateAttached(
            "CardStyle",
            typeof(Style),
            typeof(FlipperAssist),
            null);

    public static Style? GetCardStyle(BindableObject element) =>
        (Style?)element.GetValue(CardStyleProperty);

    public static void SetCardStyle(BindableObject element, Style? value) =>
        element.SetValue(CardStyleProperty, value);
}
