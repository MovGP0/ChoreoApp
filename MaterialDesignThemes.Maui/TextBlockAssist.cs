namespace MaterialDesignThemes.Maui;

/// <summary>
/// Lightweight placeholder for WPF TextBlockAssist. Provides the AutoToolTip attached property to keep XAML compatibility.
/// Business logic is factored into partial methods so platform-specific behaviour can be added without touching the public API.
/// </summary>
public static partial class TextBlockAssist
{
    public static readonly BindableProperty AutoToolTipProperty =
        BindableProperty.CreateAttached(
            "AutoToolTip",
            typeof(bool),
            typeof(TextBlockAssist),
            false,
            propertyChanged: OnAutoToolTipChanged);

    public static void SetAutoToolTip(BindableObject element, bool value) =>
        element.SetValue(AutoToolTipProperty, value);

    public static bool GetAutoToolTip(BindableObject element) =>
        (bool)element.GetValue(AutoToolTipProperty);

    private static void OnAutoToolTipChanged(BindableObject bindable, object oldValue, object newValue) =>
        OnAutoToolTipChangedPartial(bindable, (bool)newValue);

    static partial void OnAutoToolTipChangedPartial(BindableObject bindable, bool enabled);
}
