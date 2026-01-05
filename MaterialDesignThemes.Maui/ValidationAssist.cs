namespace MaterialDesignThemes.Maui;

public static class ValidationAssist
{
    public static readonly BindableProperty OnlyShowOnFocusProperty = BindableProperty.CreateAttached(
        "OnlyShowOnFocus",
        typeof(bool),
        typeof(ValidationAssist),
        false);

    public static bool GetOnlyShowOnFocus(BindableObject element)
    {
        return (bool)element.GetValue(OnlyShowOnFocusProperty);
    }

    public static void SetOnlyShowOnFocus(BindableObject element, bool value)
    {
        element.SetValue(OnlyShowOnFocusProperty, value);
    }

    public static readonly BindableProperty UsePopupProperty = BindableProperty.CreateAttached(
        "UsePopup",
        typeof(bool),
        typeof(ValidationAssist),
        false);

    public static bool GetUsePopup(BindableObject element)
    {
        return (bool)element.GetValue(UsePopupProperty);
    }

    public static void SetUsePopup(BindableObject element, bool value)
    {
        element.SetValue(UsePopupProperty, value);
    }

    public static readonly BindableProperty PopupPlacementProperty = BindableProperty.CreateAttached(
        "PopupPlacement",
        typeof(ValidationPopupPlacement),
        typeof(ValidationAssist),
        ValidationPopupPlacement.Bottom);

    public static ValidationPopupPlacement GetPopupPlacement(BindableObject element)
    {
        return (ValidationPopupPlacement)element.GetValue(PopupPlacementProperty);
    }

    public static void SetPopupPlacement(BindableObject element, ValidationPopupPlacement value)
    {
        element.SetValue(PopupPlacementProperty, value);
    }

    public static readonly BindableProperty SuppressProperty = BindableProperty.CreateAttached(
        "Suppress",
        typeof(bool),
        typeof(ValidationAssist),
        false);

    public static void SetSuppress(BindableObject element, bool value)
    {
        element.SetValue(SuppressProperty, value);
    }

    public static bool GetSuppress(BindableObject element)
    {
        return (bool)element.GetValue(SuppressProperty);
    }

    public static readonly BindableProperty BackgroundProperty = BindableProperty.CreateAttached(
        "Background",
        typeof(Brush),
        typeof(ValidationAssist),
        new SolidColorBrush(Colors.Transparent));

    public static void SetBackground(BindableObject element, Brush value)
    {
        element.SetValue(BackgroundProperty, value);
    }

    public static Brush GetBackground(BindableObject element)
    {
        return (Brush)element.GetValue(BackgroundProperty);
    }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.CreateAttached(
        "FontSize",
        typeof(double),
        typeof(ValidationAssist),
        10d);

    public static void SetFontSize(BindableObject element, double value)
    {
        element.SetValue(FontSizeProperty, value);
    }

    public static double GetFontSize(BindableObject element)
    {
        return (double)element.GetValue(FontSizeProperty);
    }

    public static readonly BindableProperty HasErrorProperty = BindableProperty.CreateAttached(
        "HasError",
        typeof(bool),
        typeof(ValidationAssist),
        false);

    public static void SetHasError(BindableObject element, bool value)
    {
        element.SetValue(HasErrorProperty, value);
    }

    public static bool GetHasError(BindableObject element)
    {
        return (bool)element.GetValue(HasErrorProperty);
    }

    public static readonly BindableProperty HorizontalAlignmentProperty = BindableProperty.CreateAttached(
        "HorizontalAlignment",
        typeof(LayoutAlignment),
        typeof(ValidationAssist),
        LayoutAlignment.Start);

    public static void SetHorizontalAlignment(BindableObject element, LayoutAlignment value)
    {
        element.SetValue(HorizontalAlignmentProperty, value);
    }

    public static LayoutAlignment GetHorizontalAlignment(BindableObject element)
    {
        return (LayoutAlignment)element.GetValue(HorizontalAlignmentProperty);
    }
}

public enum ValidationPopupPlacement
{
    Top,
    Bottom,
    Left,
    Right
}
