namespace MaterialDesignThemes.Maui;

public static class NavigationDrawerAssist
{
    private static readonly CornerRadius DefaultCornerRadius = new(2.0);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.CreateAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(NavigationDrawerAssist),
            DefaultCornerRadius);

    public static CornerRadius GetCornerRadius(BindableObject element) =>
        (CornerRadius)element.GetValue(CornerRadiusProperty);

    public static void SetCornerRadius(BindableObject element, CornerRadius value) =>
        element.SetValue(CornerRadiusProperty, value);

    public static readonly BindableProperty UnselectedIconProperty =
        BindableProperty.CreateAttached(
            "UnselectedIcon",
            typeof(PackIconKind),
            typeof(NavigationDrawerAssist),
            PackIconKind.None);

    public static PackIconKind GetUnselectedIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(UnselectedIconProperty);

    public static void SetUnselectedIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(UnselectedIconProperty, value);

    public static readonly BindableProperty SelectedIconProperty =
        BindableProperty.CreateAttached(
            "SelectedIcon",
            typeof(PackIconKind),
            typeof(NavigationDrawerAssist),
            PackIconKind.None);

    public static PackIconKind GetSelectedIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(SelectedIconProperty);

    public static void SetSelectedIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(SelectedIconProperty, value);

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.CreateAttached(
            "IconSize",
            typeof(int),
            typeof(NavigationDrawerAssist),
            24);

    public static int GetIconSize(BindableObject element) =>
        (int)element.GetValue(IconSizeProperty);

    public static void SetIconSize(BindableObject element, int value) =>
        element.SetValue(IconSizeProperty, value);
}
