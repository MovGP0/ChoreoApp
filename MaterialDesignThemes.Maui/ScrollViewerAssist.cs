namespace MaterialDesignThemes.Maui;

public static class ScrollViewerAssist
{
    internal static readonly BindableProperty HorizontalOffsetProperty = BindableProperty.CreateAttached(
        "SyncHorizontalOffset",
        typeof(double),
        typeof(ScrollViewerAssist),
        0d,
        propertyChanged: OnSyncHorizontalOffsetChanged);

    private static void OnSyncHorizontalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ScrollView scrollView)
        {
            scrollView.ScrollToAsync((double)newValue, scrollView.ScrollY, false);
        }
    }

    internal static void SetSyncHorizontalOffset(BindableObject element, double value) =>
        element.SetValue(HorizontalOffsetProperty, value);

    internal static double GetSyncHorizontalOffset(BindableObject element) =>
        (double)element.GetValue(HorizontalOffsetProperty);

    public static readonly BindableProperty IsAutoHideEnabledProperty = BindableProperty.CreateAttached(
        "IsAutoHideEnabled",
        typeof(bool),
        typeof(ScrollViewerAssist),
        false);

    public static void SetIsAutoHideEnabled(BindableObject element, bool value) =>
        element.SetValue(IsAutoHideEnabledProperty, value);

    public static bool GetIsAutoHideEnabled(BindableObject element) =>
        (bool)element.GetValue(IsAutoHideEnabledProperty);

    public static readonly BindableProperty CornerRectangleVisibilityProperty = BindableProperty.CreateAttached(
        "CornerRectangleVisibility",
        typeof(Visibility),
        typeof(ScrollViewerAssist),
        Visibility.Visible);

    public static void SetCornerRectangleVisibility(BindableObject element, Visibility value) =>
        element.SetValue(CornerRectangleVisibilityProperty, value);

    public static Visibility GetCornerRectangleVisibility(BindableObject element) =>
        (Visibility)element.GetValue(CornerRectangleVisibilityProperty);

    public static readonly BindableProperty ShowSeparatorsProperty = BindableProperty.CreateAttached(
        "ShowSeparators",
        typeof(bool),
        typeof(ScrollViewerAssist),
        false);

    public static void SetShowSeparators(BindableObject element, bool value) =>
        element.SetValue(ShowSeparatorsProperty, value);

    public static bool GetShowSeparators(BindableObject element) =>
        (bool)element.GetValue(ShowSeparatorsProperty);

    public static readonly BindableProperty PaddingModeProperty = BindableProperty.CreateAttached(
        "PaddingMode",
        typeof(PaddingMode),
        typeof(ScrollViewerAssist),
        PaddingMode.Content);

    public static void SetPaddingMode(BindableObject element, PaddingMode value) =>
        element.SetValue(PaddingModeProperty, value);

    public static PaddingMode GetPaddingMode(BindableObject element) =>
        (PaddingMode)element.GetValue(PaddingModeProperty);

    public static readonly BindableProperty IgnorePaddingProperty = BindableProperty.CreateAttached(
        "IgnorePadding",
        typeof(bool),
        typeof(ScrollViewerAssist),
        true);

    public static void SetIgnorePadding(BindableObject element, bool value) =>
        element.SetValue(IgnorePaddingProperty, value);

    public static bool GetIgnorePadding(BindableObject element) =>
        (bool)element.GetValue(IgnorePaddingProperty);

    public static readonly BindableProperty SupportHorizontalScrollProperty = BindableProperty.CreateAttached(
        "SupportHorizontalScroll",
        typeof(bool),
        typeof(ScrollViewerAssist),
        false);

    public static void SetSupportHorizontalScroll(BindableObject element, bool value) =>
        element.SetValue(SupportHorizontalScrollProperty, value);

    public static bool GetSupportHorizontalScroll(BindableObject element) =>
        (bool)element.GetValue(SupportHorizontalScrollProperty);

    public static readonly BindableProperty BubbleVerticalScrollProperty = BindableProperty.CreateAttached(
        "BubbleVerticalScroll",
        typeof(bool),
        typeof(ScrollViewerAssist),
        false);

    public static void SetBubbleVerticalScroll(BindableObject element, bool value) =>
        element.SetValue(BubbleVerticalScrollProperty, value);

    public static bool GetBubbleVerticalScroll(BindableObject element) =>
        (bool)element.GetValue(BubbleVerticalScrollProperty);
}
