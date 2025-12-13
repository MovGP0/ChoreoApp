using Microsoft.Maui.Graphics;

namespace ChoreoApp.Styling;

/// <summary>
/// Attached properties to provide floating hints/helper text behavior for inputs.
/// Ported from MaterialDesignThemes.Wpf HintAssist to MAUI.
/// </summary>
public static class HintAssist
{
    private const double DefaultFloatingScale = 0.74;
    private const double DefaultHintOpacity = 0.56;
    private static readonly Point DefaultFloatingOffset = new(0, 0);
    private static readonly Brush DefaultBackground = new SolidColorBrush(Colors.Transparent);
    private const double DefaultHelperTextFontSize = 10;

    public static readonly BindableProperty IsFloatingProperty =
        BindableProperty.CreateAttached(
            "IsFloating",
            typeof(bool),
            typeof(HintAssist),
            false);

    public static bool GetIsFloating(BindableObject element) =>
        (bool)element.GetValue(IsFloatingProperty);
    public static void SetIsFloating(BindableObject element, bool value) =>
        element.SetValue(IsFloatingProperty, value);

    public static readonly BindableProperty FloatingScaleProperty =
        BindableProperty.CreateAttached(
            "FloatingScale",
            typeof(double),
            typeof(HintAssist),
            DefaultFloatingScale);

    public static double GetFloatingScale(BindableObject element) =>
        (double)element.GetValue(FloatingScaleProperty);
    public static void SetFloatingScale(BindableObject element, double value) =>
        element.SetValue(FloatingScaleProperty, value);

    public static readonly BindableProperty FloatingOffsetProperty =
        BindableProperty.CreateAttached(
            "FloatingOffset",
            typeof(Point),
            typeof(HintAssist),
            DefaultFloatingOffset);

    public static Point GetFloatingOffset(BindableObject element) =>
        (Point)element.GetValue(FloatingOffsetProperty);
    public static void SetFloatingOffset(BindableObject element, Point value) =>
        element.SetValue(FloatingOffsetProperty, value);

    public static readonly BindableProperty HintProperty =
        BindableProperty.CreateAttached(
            "Hint",
            typeof(object),
            typeof(HintAssist),
            null);

    public static object? GetHint(BindableObject element) =>
        element.GetValue(HintProperty);
    public static void SetHint(BindableObject element, object? value) =>
        element.SetValue(HintProperty, value);

    public static readonly BindableProperty HintOpacityProperty =
        BindableProperty.CreateAttached(
            "HintOpacity",
            typeof(double),
            typeof(HintAssist),
            DefaultHintOpacity);

    public static double GetHintOpacity(BindableObject element) =>
        (double)element.GetValue(HintOpacityProperty);
    public static void SetHintOpacity(BindableObject element, double value) =>
        element.SetValue(HintOpacityProperty, value);

    public static readonly BindableProperty HintHorizontalAlignmentProperty =
        BindableProperty.CreateAttached(
            "HintHorizontalAlignment",
            typeof(LayoutAlignment),
            typeof(HintAssist),
            LayoutAlignment.Start);

    public static LayoutAlignment GetHintHorizontalAlignment(BindableObject element) =>
        (LayoutAlignment)element.GetValue(HintHorizontalAlignmentProperty);
    public static void SetHintHorizontalAlignment(BindableObject element, LayoutAlignment value) =>
        element.SetValue(HintHorizontalAlignmentProperty, value);

    public static readonly BindableProperty FloatingHintHorizontalAlignmentProperty =
        BindableProperty.CreateAttached(
            "FloatingHintHorizontalAlignment",
            typeof(LayoutAlignment),
            typeof(HintAssist),
            LayoutAlignment.Start);

    public static LayoutAlignment GetFloatingHintHorizontalAlignment(BindableObject element) =>
        (LayoutAlignment)element.GetValue(FloatingHintHorizontalAlignmentProperty);
    public static void SetFloatingHintHorizontalAlignment(BindableObject element, LayoutAlignment value) =>
        element.SetValue(FloatingHintHorizontalAlignmentProperty, value);

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.CreateAttached(
            "FontFamily",
            typeof(string),
            typeof(HintAssist),
            null);

    public static string? GetFontFamily(BindableObject element) =>
        (string?)element.GetValue(FontFamilyProperty);
    public static void SetFontFamily(BindableObject element, string? value) =>
        element.SetValue(FontFamilyProperty, value);

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.CreateAttached(
            "Foreground",
            typeof(Brush),
            typeof(HintAssist),
            null);

    public static Brush? GetForeground(BindableObject element) =>
        (Brush?)element.GetValue(ForegroundProperty);
    public static void SetForeground(BindableObject element, Brush? value) =>
        element.SetValue(ForegroundProperty, value);

    public static readonly BindableProperty BackgroundProperty =
        BindableProperty.CreateAttached(
            "Background",
            typeof(Brush),
            typeof(HintAssist),
            DefaultBackground);

    public static Brush GetBackground(BindableObject element) =>
        (Brush)element.GetValue(BackgroundProperty);
    public static void SetBackground(BindableObject element, Brush value) =>
        element.SetValue(BackgroundProperty, value);

    public static readonly BindableProperty HintPaddingBrushProperty =
        BindableProperty.CreateAttached(
            "HintPaddingBrush",
            typeof(Brush),
            typeof(HintAssist),
            null);

    public static Brush? GetHintPaddingBrush(BindableObject element) =>
        (Brush?)element.GetValue(HintPaddingBrushProperty);
    public static void SetHintPaddingBrush(BindableObject element, Brush? value) =>
        element.SetValue(HintPaddingBrushProperty, value);

    public static readonly BindableProperty ApplyHintPaddingBrushProperty =
        BindableProperty.CreateAttached(
            "ApplyHintPaddingBrush",
            typeof(bool),
            typeof(HintAssist),
            false);

    public static bool GetApplyHintPaddingBrush(BindableObject element) =>
        (bool)element.GetValue(ApplyHintPaddingBrushProperty);
    public static void SetApplyHintPaddingBrush(BindableObject element, bool value) =>
        element.SetValue(ApplyHintPaddingBrushProperty, value);

    public static readonly BindableProperty HelperTextProperty =
        BindableProperty.CreateAttached(
            "HelperText",
            typeof(string),
            typeof(HintAssist),
            null);

    public static string? GetHelperText(BindableObject element) =>
        (string?)element.GetValue(HelperTextProperty);
    public static void SetHelperText(BindableObject element, string? value) =>
        element.SetValue(HelperTextProperty, value);

    public static readonly BindableProperty HelperTextFontSizeProperty =
        BindableProperty.CreateAttached(
            "HelperTextFontSize",
            typeof(double),
            typeof(HintAssist),
            DefaultHelperTextFontSize);

    public static double GetHelperTextFontSize(BindableObject element) =>
        (double)element.GetValue(HelperTextFontSizeProperty);
    public static void SetHelperTextFontSize(BindableObject element, double value) =>
        element.SetValue(HelperTextFontSizeProperty, value);

    public static readonly BindableProperty HelperTextStyleProperty =
        BindableProperty.CreateAttached(
            "HelperTextStyle",
            typeof(Style),
            typeof(HintAssist),
            null);

    public static Style? GetHelperTextStyle(BindableObject element) =>
        (Style?)element.GetValue(HelperTextStyleProperty);
    public static void SetHelperTextStyle(BindableObject element, Style? value) =>
        element.SetValue(HelperTextStyleProperty, value);
}
