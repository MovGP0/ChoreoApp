namespace MaterialDesignThemes.Maui;

public sealed class BottomDashedLineAdorner
{
    private static readonly Thickness DefaultThickness = new(1);
    private const double DefaultThicknessScale = 1.33;
    private const double DefaultOpacity = 0.56;

    public static readonly BindableProperty IsAttachedProperty = BindableProperty.CreateAttached(
        "IsAttached",
        typeof(bool),
        typeof(BottomDashedLineAdorner),
        false);

    public static bool GetIsAttached(BindableObject element) =>
        (bool)element.GetValue(IsAttachedProperty);

    public static void SetIsAttached(BindableObject element, bool value) =>
        element.SetValue(IsAttachedProperty, value);

    public static readonly BindableProperty BrushProperty = BindableProperty.CreateAttached(
        "Brush",
        typeof(Brush),
        typeof(BottomDashedLineAdorner),
        null);

    public static Brush? GetBrush(BindableObject element) =>
        (Brush?)element.GetValue(BrushProperty);

    public static void SetBrush(BindableObject element, Brush? value) =>
        element.SetValue(BrushProperty, value);

    public static readonly BindableProperty ThicknessProperty = BindableProperty.CreateAttached(
        "Thickness",
        typeof(Thickness),
        typeof(BottomDashedLineAdorner),
        DefaultThickness);

    public static Thickness GetThickness(BindableObject element) =>
        (Thickness)element.GetValue(ThicknessProperty);

    public static void SetThickness(BindableObject element, Thickness value) =>
        element.SetValue(ThicknessProperty, value);

    public static readonly BindableProperty ThicknessScaleProperty = BindableProperty.CreateAttached(
        "ThicknessScale",
        typeof(double),
        typeof(BottomDashedLineAdorner),
        DefaultThicknessScale);

    public static double GetThicknessScale(BindableObject element) =>
        (double)element.GetValue(ThicknessScaleProperty);

    public static void SetThicknessScale(BindableObject element, double value) =>
        element.SetValue(ThicknessScaleProperty, value);

    public static readonly BindableProperty BrushOpacityProperty = BindableProperty.CreateAttached(
        "BrushOpacity",
        typeof(double),
        typeof(BottomDashedLineAdorner),
        DefaultOpacity);

    public static double GetBrushOpacity(BindableObject element) =>
        (double)element.GetValue(BrushOpacityProperty);

    public static void SetBrushOpacity(BindableObject element, double value) =>
        element.SetValue(BrushOpacityProperty, value);

    public static readonly BindableProperty DashStyleProperty = BindableProperty.CreateAttached(
        "DashStyle",
        typeof(DoubleCollection),
        typeof(BottomDashedLineAdorner),
        null);

    public static void SetDashStyle(BindableObject element, DoubleCollection? value) =>
        element.SetValue(DashStyleProperty, value);

    public static DoubleCollection? GetDashStyle(BindableObject element) =>
        (DoubleCollection?)element.GetValue(DashStyleProperty);
}
