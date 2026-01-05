namespace MaterialDesignThemes.Maui;

public static class ButtonProgressAssist
{
    private const double DefaultMaximum = 100.0;

    public static readonly BindableProperty MinimumProperty = BindableProperty.CreateAttached(
        "Minimum",
        typeof(double),
        typeof(ButtonProgressAssist),
        0);

    public static double GetMinimum(BindableObject element) =>
        (double)element.GetValue(MinimumProperty);

    public static void SetMinimum(BindableObject element, double value) =>
        element.SetValue(MinimumProperty, value);

    public static readonly BindableProperty MaximumProperty = BindableProperty.CreateAttached(
        "Maximum",
        typeof(double),
        typeof(ButtonProgressAssist),
        DefaultMaximum);

    public static double GetMaximum(BindableObject element) =>
        (double)element.GetValue(MaximumProperty);

    public static void SetMaximum(BindableObject element, double value) =>
        element.SetValue(MaximumProperty, value);

    public static readonly BindableProperty ValueProperty = BindableProperty.CreateAttached(
        "Value",
        typeof(double),
        typeof(ButtonProgressAssist),
        0);

    public static double GetValue(BindableObject element) =>
        (double)element.GetValue(ValueProperty);

    public static void SetValue(BindableObject element, double value) =>
        element.SetValue(ValueProperty, value);

    public static readonly BindableProperty IsIndeterminateProperty = BindableProperty.CreateAttached(
        "IsIndeterminate",
        typeof(bool),
        typeof(ButtonProgressAssist),
        false);

    public static bool GetIsIndeterminate(BindableObject element) =>
        (bool)element.GetValue(IsIndeterminateProperty);

    public static void SetIsIndeterminate(BindableObject element, bool value) =>
        element.SetValue(IsIndeterminateProperty, value);

    public static readonly BindableProperty IndicatorForegroundProperty = BindableProperty.CreateAttached(
        "IndicatorForeground",
        typeof(Brush),
        typeof(ButtonProgressAssist),
        null);

    public static Brush? GetIndicatorForeground(BindableObject element) =>
        (Brush?)element.GetValue(IndicatorForegroundProperty);

    public static void SetIndicatorForeground(BindableObject element, Brush? value) =>
        element.SetValue(IndicatorForegroundProperty, value);

    public static readonly BindableProperty IndicatorBackgroundProperty = BindableProperty.CreateAttached(
        "IndicatorBackground",
        typeof(Brush),
        typeof(ButtonProgressAssist),
        null);

    public static Brush? GetIndicatorBackground(BindableObject element) =>
        (Brush?)element.GetValue(IndicatorBackgroundProperty);

    public static void SetIndicatorBackground(BindableObject element, Brush? value) =>
        element.SetValue(IndicatorBackgroundProperty, value);

    public static readonly BindableProperty IsIndicatorVisibleProperty = BindableProperty.CreateAttached(
        "IsIndicatorVisible",
        typeof(bool),
        typeof(ButtonProgressAssist),
        false);

    public static bool GetIsIndicatorVisible(BindableObject element) =>
        (bool)element.GetValue(IsIndicatorVisibleProperty);

    public static void SetIsIndicatorVisible(BindableObject element, bool value) =>
        element.SetValue(IsIndicatorVisibleProperty, value);

    public static readonly BindableProperty OpacityProperty = BindableProperty.CreateAttached(
        "Opacity",
        typeof(double),
        typeof(ButtonProgressAssist),
        0);

    public static double GetOpacity(BindableObject element) =>
        (double)element.GetValue(OpacityProperty);

    public static void SetOpacity(BindableObject element, double value) =>
        element.SetValue(OpacityProperty, value);
}
