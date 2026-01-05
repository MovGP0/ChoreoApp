namespace MaterialDesignThemes.Maui;

/// <summary>
/// Attached property placeholders mirroring WPF TextFieldAssist for XAML compatibility in MAUI.
/// Behaviour (e.g., underline rendering, context menu) is not implemented; properties are available for binding/styling.
/// </summary>
public static partial class TextFieldAssist
{
    public static readonly BindableProperty TextBoxViewMarginProperty =
        BindableProperty.CreateAttached(
            "TextBoxViewMargin",
            typeof(Thickness),
            typeof(TextFieldAssist),
            new Thickness(double.NaN));

    public static void SetTextBoxViewMargin(BindableObject element, Thickness value) =>
        element.SetValue(TextBoxViewMarginProperty, value);
    public static Thickness GetTextBoxViewMargin(BindableObject element) =>
        (Thickness)element.GetValue(TextBoxViewMarginProperty);

    public static readonly BindableProperty TextBoxViewVerticalAlignmentProperty =
        BindableProperty.CreateAttached(
            "TextBoxViewVerticalAlignment",
            typeof(LayoutAlignment),
            typeof(TextFieldAssist),
            LayoutAlignment.Fill);

    public static void SetTextBoxViewVerticalAlignment(BindableObject element, LayoutAlignment value) =>
        element.SetValue(TextBoxViewVerticalAlignmentProperty, value);
    public static LayoutAlignment GetTextBoxViewVerticalAlignment(BindableObject element) =>
        (LayoutAlignment)element.GetValue(TextBoxViewVerticalAlignmentProperty);

    public static readonly BindableProperty DecorationVisibilityProperty =
        BindableProperty.CreateAttached(
            "DecorationVisibility",
            typeof(bool),
            typeof(TextFieldAssist),
            true);

    public static void SetDecorationVisibility(BindableObject element, bool value) =>
        element.SetValue(DecorationVisibilityProperty, value);
    public static bool GetDecorationVisibility(BindableObject element) =>
        (bool)element.GetValue(DecorationVisibilityProperty);

    public static readonly BindableProperty UnderlineBrushProperty =
        BindableProperty.CreateAttached(
            "UnderlineBrush",
            typeof(Brush),
            typeof(TextFieldAssist),
            new SolidColorBrush(Colors.Transparent));

    public static void SetUnderlineBrush(BindableObject element, Brush value) =>
        element.SetValue(UnderlineBrushProperty, value);
    public static Brush GetUnderlineBrush(BindableObject element) =>
        (Brush)element.GetValue(UnderlineBrushProperty);

    public static readonly BindableProperty HasFilledTextFieldProperty =
        BindableProperty.CreateAttached(
            "HasFilledTextField",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetHasFilledTextField(BindableObject element, bool value) =>
        element.SetValue(HasFilledTextFieldProperty, value);
    public static bool GetHasFilledTextField(BindableObject element) =>
        (bool)element.GetValue(HasFilledTextFieldProperty);

    public static readonly BindableProperty HasOutlinedTextFieldProperty =
        BindableProperty.CreateAttached(
            "HasOutlinedTextField",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetHasOutlinedTextField(BindableObject element, bool value) =>
        element.SetValue(HasOutlinedTextFieldProperty, value);
    public static bool GetHasOutlinedTextField(BindableObject element) =>
        (bool)element.GetValue(HasOutlinedTextFieldProperty);

    public static readonly BindableProperty TextFieldCornerRadiusProperty =
        BindableProperty.CreateAttached(
            "TextFieldCornerRadius",
            typeof(CornerRadius),
            typeof(TextFieldAssist),
            new CornerRadius(0));

    public static void SetTextFieldCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(TextFieldCornerRadiusProperty, value);
    public static CornerRadius GetTextFieldCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(TextFieldCornerRadiusProperty);

    public static readonly BindableProperty UnderlineCornerRadiusProperty =
        BindableProperty.CreateAttached(
            "UnderlineCornerRadius",
            typeof(CornerRadius),
            typeof(TextFieldAssist),
            new CornerRadius(0));

    public static void SetUnderlineCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(UnderlineCornerRadiusProperty, value);
    public static CornerRadius GetUnderlineCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(UnderlineCornerRadiusProperty);

    public static readonly BindableProperty NewSpecHighlightingEnabledProperty =
        BindableProperty.CreateAttached(
            "NewSpecHighlightingEnabled",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetNewSpecHighlightingEnabled(BindableObject element, bool value) =>
        element.SetValue(NewSpecHighlightingEnabledProperty, value);
    public static bool GetNewSpecHighlightingEnabled(BindableObject element) =>
        (bool)element.GetValue(NewSpecHighlightingEnabledProperty);

    public static readonly BindableProperty RippleOnFocusEnabledProperty =
        BindableProperty.CreateAttached(
            "RippleOnFocusEnabled",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetRippleOnFocusEnabled(BindableObject element, bool value) =>
        element.SetValue(RippleOnFocusEnabledProperty, value);
    public static bool GetRippleOnFocusEnabled(BindableObject element) =>
        (bool)element.GetValue(RippleOnFocusEnabledProperty);

    public static readonly BindableProperty IncludeSpellingSuggestionsProperty =
        BindableProperty.CreateAttached(
            "IncludeSpellingSuggestions",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetIncludeSpellingSuggestions(BindableObject element, bool value) =>
        element.SetValue(IncludeSpellingSuggestionsProperty, value);
    public static bool GetIncludeSpellingSuggestions(BindableObject element) =>
        (bool)element.GetValue(IncludeSpellingSuggestionsProperty);

    public static readonly BindableProperty SuffixTextProperty =
        BindableProperty.CreateAttached(
            "SuffixText",
            typeof(string),
            typeof(TextFieldAssist),
            null);

    public static void SetSuffixText(BindableObject element, string? value) =>
        element.SetValue(SuffixTextProperty, value);
    public static string? GetSuffixText(BindableObject element) =>
        (string?)element.GetValue(SuffixTextProperty);

    public static readonly BindableProperty PrefixTextProperty =
        BindableProperty.CreateAttached(
            "PrefixText",
            typeof(string),
            typeof(TextFieldAssist),
            null);

    public static void SetPrefixText(BindableObject element, string? value) =>
        element.SetValue(PrefixTextProperty, value);
    public static string? GetPrefixText(BindableObject element) =>
        (string?)element.GetValue(PrefixTextProperty);

    public static readonly BindableProperty SuffixTextVisibilityProperty =
        BindableProperty.CreateAttached(
            "SuffixTextVisibility",
            typeof(bool),
            typeof(TextFieldAssist),
            true);

    public static void SetSuffixTextVisibility(BindableObject element, bool value) =>
        element.SetValue(SuffixTextVisibilityProperty, value);
    public static bool GetSuffixTextVisibility(BindableObject element) =>
        (bool)element.GetValue(SuffixTextVisibilityProperty);

    public static readonly BindableProperty PrefixTextVisibilityProperty =
        BindableProperty.CreateAttached(
            "PrefixTextVisibility",
            typeof(bool),
            typeof(TextFieldAssist),
            true);

    public static void SetPrefixTextVisibility(BindableObject element, bool value) =>
        element.SetValue(PrefixTextVisibilityProperty, value);
    public static bool GetPrefixTextVisibility(BindableObject element) =>
        (bool)element.GetValue(PrefixTextVisibilityProperty);

    public static readonly BindableProperty SuffixTextHintBehaviorProperty =
        BindableProperty.CreateAttached(
            "SuffixTextHintBehavior",
            typeof(int),
            typeof(TextFieldAssist),
            0);

    public static void SetSuffixTextHintBehavior(BindableObject element, int value) =>
        element.SetValue(SuffixTextHintBehaviorProperty, value);
    public static int GetSuffixTextHintBehavior(BindableObject element) =>
        (int)element.GetValue(SuffixTextHintBehaviorProperty);

    public static readonly BindableProperty PrefixTextHintBehaviorProperty =
        BindableProperty.CreateAttached(
            "PrefixTextHintBehavior",
            typeof(int),
            typeof(TextFieldAssist),
            0);

    public static void SetPrefixTextHintBehavior(BindableObject element, int value) =>
        element.SetValue(PrefixTextHintBehaviorProperty, value);
    public static int GetPrefixTextHintBehavior(BindableObject element) =>
        (int)element.GetValue(PrefixTextHintBehaviorProperty);

    public static readonly BindableProperty HasClearButtonProperty =
        BindableProperty.CreateAttached(
            "HasClearButton",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetHasClearButton(BindableObject element, bool value) =>
        element.SetValue(HasClearButtonProperty, value);
    public static bool GetHasClearButton(BindableObject element) =>
        (bool)element.GetValue(HasClearButtonProperty);

    public static readonly BindableProperty ClearButtonSizeProperty =
        BindableProperty.CreateAttached(
            "ClearButtonSize",
            typeof(double),
            typeof(TextFieldAssist),
            16.0d);

    public static void SetClearButtonSize(BindableObject element, double value) =>
        element.SetValue(ClearButtonSizeProperty, value);
    public static double GetClearButtonSize(BindableObject element) =>
        (double)element.GetValue(ClearButtonSizeProperty);

    public static readonly BindableProperty HasLeadingIconProperty =
        BindableProperty.CreateAttached(
            "HasLeadingIcon",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetHasLeadingIcon(BindableObject element, bool value) =>
        element.SetValue(HasLeadingIconProperty, value);
    public static bool GetHasLeadingIcon(BindableObject element) =>
        (bool)element.GetValue(HasLeadingIconProperty);

    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.CreateAttached(
            "LeadingIcon",
            typeof(PackIconKind),
            typeof(TextFieldAssist),
            default(PackIconKind));

    public static void SetLeadingIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(LeadingIconProperty, value);
    public static PackIconKind GetLeadingIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(LeadingIconProperty);

    public static readonly BindableProperty LeadingIconSizeProperty =
        BindableProperty.CreateAttached(
            "LeadingIconSize",
            typeof(double),
            typeof(TextFieldAssist),
            20.0d);

    public static void SetLeadingIconSize(BindableObject element, double value) =>
        element.SetValue(LeadingIconSizeProperty, value);
    public static double GetLeadingIconSize(BindableObject element) =>
        (double)element.GetValue(LeadingIconSizeProperty);

    public static readonly BindableProperty HasTrailingIconProperty =
        BindableProperty.CreateAttached(
            "HasTrailingIcon",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetHasTrailingIcon(BindableObject element, bool value) =>
        element.SetValue(HasTrailingIconProperty, value);
    public static bool GetHasTrailingIcon(BindableObject element) =>
        (bool)element.GetValue(HasTrailingIconProperty);

    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.CreateAttached(
            "TrailingIcon",
            typeof(PackIconKind),
            typeof(TextFieldAssist),
            default(PackIconKind));

    public static void SetTrailingIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(TrailingIconProperty, value);
    public static PackIconKind GetTrailingIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(TrailingIconProperty);

    public static readonly BindableProperty TrailingIconSizeProperty =
        BindableProperty.CreateAttached(
            "TrailingIconSize",
            typeof(double),
            typeof(TextFieldAssist),
            20.0d);

    public static void SetTrailingIconSize(BindableObject element, double value) =>
        element.SetValue(TrailingIconSizeProperty, value);
    public static double GetTrailingIconSize(BindableObject element) =>
        (double)element.GetValue(TrailingIconSizeProperty);

    public static readonly BindableProperty IconVerticalAlignmentProperty =
        BindableProperty.CreateAttached(
            "IconVerticalAlignment",
            typeof(LayoutAlignment),
            typeof(TextFieldAssist),
            LayoutAlignment.Center);

    public static void SetIconVerticalAlignment(BindableObject element, LayoutAlignment value) =>
        element.SetValue(IconVerticalAlignmentProperty, value);
    public static LayoutAlignment GetIconVerticalAlignment(BindableObject element) =>
        (LayoutAlignment)element.GetValue(IconVerticalAlignmentProperty);

    public static readonly BindableProperty CharacterCounterStyleProperty =
        BindableProperty.CreateAttached(
            "CharacterCounterStyle",
            typeof(Style),
            typeof(TextFieldAssist),
            null);

    public static void SetCharacterCounterStyle(BindableObject element, Style? value) =>
        element.SetValue(CharacterCounterStyleProperty, value);
    public static Style? GetCharacterCounterStyle(BindableObject element) =>
        (Style?)element.GetValue(CharacterCounterStyleProperty);

    public static readonly BindableProperty CharacterCounterVisibilityProperty =
        BindableProperty.CreateAttached(
            "CharacterCounterVisibility",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetCharacterCounterVisibility(BindableObject element, bool value) =>
        element.SetValue(CharacterCounterVisibilityProperty, value);
    public static bool GetCharacterCounterVisibility(BindableObject element) =>
        (bool)element.GetValue(CharacterCounterVisibilityProperty);

    public static readonly BindableProperty OutlinedBorderActiveThicknessProperty =
        BindableProperty.CreateAttached(
            "OutlinedBorderActiveThickness",
            typeof(Thickness),
            typeof(TextFieldAssist),
            new Thickness(1));

    public static void SetOutlinedBorderActiveThickness(BindableObject element, Thickness value) =>
        element.SetValue(OutlinedBorderActiveThicknessProperty, value);
    public static Thickness GetOutlinedBorderActiveThickness(BindableObject element) =>
        (Thickness)element.GetValue(OutlinedBorderActiveThicknessProperty);

    public static readonly BindableProperty TextBoxLineCountProperty =
        BindableProperty.CreateAttached(
            "TextBoxLineCount",
            typeof(int),
            typeof(TextFieldAssist),
            0);

    public static void SetTextBoxLineCount(BindableObject element, int value) =>
        element.SetValue(TextBoxLineCountProperty, value);
    public static int GetTextBoxLineCount(BindableObject element) =>
        (int)element.GetValue(TextBoxLineCountProperty);

    public static readonly BindableProperty TextBoxIsMultiLineProperty =
        BindableProperty.CreateAttached(
            "TextBoxIsMultiLine",
            typeof(bool),
            typeof(TextFieldAssist),
            false);

    public static void SetTextBoxIsMultiLine(BindableObject element, bool value) =>
        element.SetValue(TextBoxIsMultiLineProperty, value);

    public static bool GetTextBoxIsMultiLine(BindableObject element) =>
        (bool)element.GetValue(TextBoxIsMultiLineProperty);
}
