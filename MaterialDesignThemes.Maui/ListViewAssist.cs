namespace MaterialDesignThemes.Maui;

public static class ListViewAssist
{
    public static readonly BindableProperty ListViewItemPaddingProperty =
        BindableProperty.CreateAttached(
            "ListViewItemPadding",
            typeof(Thickness),
            typeof(ListViewAssist),
            new Thickness(8, 8, 8, 8));

    public static void SetListViewItemPadding(BindableObject element, Thickness value) =>
        element.SetValue(ListViewItemPaddingProperty, value);

    public static Thickness GetListViewItemPadding(BindableObject element) =>
        (Thickness)element.GetValue(ListViewItemPaddingProperty);

    public static readonly BindableProperty HeaderRowBackgroundProperty =
        BindableProperty.CreateAttached(
            "HeaderRowBackground",
            typeof(Brush),
            typeof(ListViewAssist),
            null);

    public static void SetHeaderRowBackground(BindableObject element, Brush value) =>
        element.SetValue(HeaderRowBackgroundProperty, value);

    public static Brush? GetHeaderRowBackground(BindableObject element) =>
        (Brush?)element.GetValue(HeaderRowBackgroundProperty);
}
