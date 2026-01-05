namespace MaterialDesignThemes.Maui;

public static class NavigationBarAssist
{
    private static readonly CornerRadius DefaultCornerRadius = new(2.0);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.CreateAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(NavigationBarAssist),
            DefaultCornerRadius);

    public static CornerRadius GetCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(CornerRadiusProperty);

    public static void SetCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(CornerRadiusProperty, value);

    public static readonly BindableProperty ShowSelectionBackgroundProperty =
        BindableProperty.CreateAttached(
            "ShowSelectionBackground",
            typeof(bool),
            typeof(NavigationBarAssist),
            false);

    public static bool GetShowSelectionBackground(BindableObject element) =>
        (bool)element.GetValue(ShowSelectionBackgroundProperty);

    public static void SetShowSelectionBackground(BindableObject element, bool value) =>
        element.SetValue(ShowSelectionBackgroundProperty, value);

    public static readonly BindableProperty SelectionCornerRadiusProperty =
        BindableProperty.CreateAttached(
            "SelectionCornerRadius",
            typeof(CornerRadius),
            typeof(NavigationBarAssist),
            default(CornerRadius));

    public static CornerRadius GetSelectionCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(SelectionCornerRadiusProperty);

    public static void SetSelectionCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(SelectionCornerRadiusProperty, value);

    public static readonly BindableProperty SelectionHeightProperty =
        BindableProperty.CreateAttached(
            "SelectionHeight",
            typeof(int),
            typeof(NavigationBarAssist),
            default(int));

    public static int GetSelectionHeight(BindableObject element) =>
        (int)element.GetValue(SelectionHeightProperty);

    public static void SetSelectionHeight(BindableObject element, int value) =>
        element.SetValue(SelectionHeightProperty, value);

    public static readonly BindableProperty SelectionWidthProperty =
        BindableProperty.CreateAttached(
            "SelectionWidth",
            typeof(int),
            typeof(NavigationBarAssist),
            default(int));

    public static int GetSelectionWidth(BindableObject element) =>
        (int)element.GetValue(SelectionWidthProperty);

    public static void SetSelectionWidth(BindableObject element, int value) =>
        element.SetValue(SelectionWidthProperty, value);

    public static readonly BindableProperty UnselectedIconProperty =
        BindableProperty.CreateAttached(
            "UnselectedIcon",
            typeof(PackIconKind),
            typeof(NavigationBarAssist),
            PackIconKind.None);

    public static PackIconKind GetUnselectedIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(UnselectedIconProperty);

    public static void SetUnselectedIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(UnselectedIconProperty, value);

    public static readonly BindableProperty SelectedIconProperty =
        BindableProperty.CreateAttached(
            "SelectedIcon",
            typeof(PackIconKind),
            typeof(NavigationBarAssist),
            PackIconKind.None);

    public static PackIconKind GetSelectedIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(SelectedIconProperty);

    public static void SetSelectedIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(SelectedIconProperty, value);

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.CreateAttached(
            "IconSize",
            typeof(int),
            typeof(NavigationBarAssist),
            24);

    public static int GetIconSize(BindableObject element) =>
        (int)element.GetValue(IconSizeProperty);

    public static void SetIconSize(BindableObject element, int value) =>
        element.SetValue(IconSizeProperty, value);

    public static readonly BindableProperty IsTextVisibleProperty =
        BindableProperty.CreateAttached(
            "IsTextVisible",
            typeof(bool),
            typeof(NavigationBarAssist),
            true);

    public static bool GetIsTextVisible(BindableObject element) =>
        (bool)element.GetValue(IsTextVisibleProperty);

    public static void SetIsTextVisible(BindableObject element, bool value) =>
        element.SetValue(IsTextVisibleProperty, value);
}
