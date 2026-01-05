namespace MaterialDesignThemes.Maui;

public enum TabControlHeaderBehavior
{
    Scrolling,
    Wrapping
}

public static class TabAssist
{
    public static readonly BindableProperty HasFilledTabProperty = BindableProperty.CreateAttached(
        "HasFilledTab",
        typeof(bool),
        typeof(TabAssist),
        false);

    public static void SetHasFilledTab(BindableObject element, bool value)
        => element.SetValue(HasFilledTabProperty, value);

    public static bool GetHasFilledTab(BindableObject element)
        => (bool)element.GetValue(HasFilledTabProperty);

    public static readonly BindableProperty HasUniformTabWidthProperty = BindableProperty.CreateAttached(
        "HasUniformTabWidth",
        typeof(bool),
        typeof(TabAssist),
        false);

    public static void SetHasUniformTabWidth(BindableObject element, bool value)
        => element.SetValue(HasUniformTabWidthProperty, value);

    public static bool GetHasUniformTabWidth(BindableObject element)
        => (bool)element.GetValue(HasUniformTabWidthProperty);

    public static readonly BindableProperty HeaderPanelMarginProperty = BindableProperty.CreateAttached(
        "HeaderPanelMargin",
        typeof(Thickness),
        typeof(TabAssist),
        default(Thickness));

    public static void SetHeaderPanelMargin(BindableObject element, Thickness value)
        => element.SetValue(HeaderPanelMarginProperty, value);

    public static Thickness GetHeaderPanelMargin(BindableObject element)
        => (Thickness)element.GetValue(HeaderPanelMarginProperty);

    public static readonly BindableProperty BindableIsItemsHostProperty = BindableProperty.CreateAttached(
        "BindableIsItemsHost",
        typeof(Visibility),
        typeof(TabAssist),
        Visibility.Collapsed);

    public static void SetBindableIsItemsHost(BindableObject element, Visibility value)
        => element.SetValue(BindableIsItemsHostProperty, value);

    public static Visibility GetBindableIsItemsHost(BindableObject element)
        => (Visibility)element.GetValue(BindableIsItemsHostProperty);

    public static readonly BindableProperty TabHeaderCursorProperty = BindableProperty.CreateAttached(
        "TabHeaderCursor",
        typeof(CursorIcon),
        typeof(TabAssist),
        CursorIcon.Hand);

    public static void SetTabHeaderCursor(BindableObject element, CursorIcon value)
        => element.SetValue(TabHeaderCursorProperty, value);

    public static CursorIcon GetTabHeaderCursor(BindableObject element)
        => (CursorIcon)element.GetValue(TabHeaderCursorProperty);

    public static readonly BindableProperty HeaderBehaviorProperty = BindableProperty.CreateAttached(
        "HeaderBehavior",
        typeof(TabControlHeaderBehavior),
        typeof(TabAssist),
        TabControlHeaderBehavior.Scrolling);

    public static void SetHeaderBehavior(BindableObject element, TabControlHeaderBehavior value)
        => element.SetValue(HeaderBehaviorProperty, value);

    public static TabControlHeaderBehavior GetHeaderBehavior(BindableObject element)
        => (TabControlHeaderBehavior)element.GetValue(HeaderBehaviorProperty);
}
