namespace MaterialDesignThemes.Maui;

public static class MenuAssist
{
    public static readonly BindableProperty TopLevelMenuItemHeightProperty =
        BindableProperty.CreateAttached(
            "TopLevelMenuItemHeight",
            typeof(double),
            typeof(MenuAssist),
            0);

    public static double GetTopLevelMenuItemHeight(BindableObject element) =>
        (double)element.GetValue(TopLevelMenuItemHeightProperty);

    public static void SetTopLevelMenuItemHeight(BindableObject element, double value) =>
        element.SetValue(TopLevelMenuItemHeightProperty, value);

    public static readonly BindableProperty MenuItemsPresenterMarginProperty =
        BindableProperty.CreateAttached(
            "MenuItemsPresenterMargin",
            typeof(Thickness),
            typeof(MenuAssist),
            new Thickness(0, 16, 0, 16));

    public static Thickness GetMenuItemsPresenterMargin(BindableObject obj) =>
        (Thickness)obj.GetValue(MenuItemsPresenterMarginProperty);

    public static void SetMenuItemsPresenterMargin(BindableObject obj, Thickness value) =>
        obj.SetValue(MenuItemsPresenterMarginProperty, value);
}
