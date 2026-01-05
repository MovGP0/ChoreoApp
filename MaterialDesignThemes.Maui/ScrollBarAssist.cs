namespace MaterialDesignThemes.Maui;

public static class ScrollBarAssist
{
    public static readonly BindableProperty ButtonsVisibilityProperty = BindableProperty.CreateAttached(
        "ButtonsVisibility",
        typeof(Visibility),
        typeof(ScrollBarAssist),
        Visibility.Visible);

    public static void SetButtonsVisibility(BindableObject element, Visibility value) =>
        element.SetValue(ButtonsVisibilityProperty, value);

    public static Visibility GetButtonsVisibility(BindableObject element) =>
        (Visibility)element.GetValue(ButtonsVisibilityProperty);

    public static readonly BindableProperty ThumbCornerRadiusProperty = BindableProperty.CreateAttached(
        "ThumbCornerRadius",
        typeof(CornerRadius),
        typeof(ScrollBarAssist),
        new CornerRadius(0));

    public static void SetThumbCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(ThumbCornerRadiusProperty, value);

    public static CornerRadius GetThumbCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(ThumbCornerRadiusProperty);

    public static readonly BindableProperty ThumbWidthProperty = BindableProperty.CreateAttached(
        "ThumbWidth",
        typeof(double),
        typeof(ScrollBarAssist),
        double.NaN);

    public static void SetThumbWidth(BindableObject element, double value) =>
        element.SetValue(ThumbWidthProperty, value);

    public static double GetThumbWidth(BindableObject element) =>
        (double)element.GetValue(ThumbWidthProperty);

    public static readonly BindableProperty ThumbHeightProperty = BindableProperty.CreateAttached(
        "ThumbHeight",
        typeof(double),
        typeof(ScrollBarAssist),
        double.NaN);

    public static void SetThumbHeight(BindableObject element, double value) =>
        element.SetValue(ThumbHeightProperty, value);

    public static double GetThumbHeight(BindableObject element) =>
        (double)element.GetValue(ThumbHeightProperty);
}
