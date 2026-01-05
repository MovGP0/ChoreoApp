namespace MaterialDesignThemes.Maui;

public class TabView : ContentView
{
    public static readonly BindableProperty TabStripBackgroundColorProperty = BindableProperty.Create(
        nameof(TabStripBackgroundColor),
        typeof(Color),
        typeof(TabView),
        Colors.Transparent);

    public Color TabStripBackgroundColor
    {
        get => (Color)GetValue(TabStripBackgroundColorProperty);
        set => SetValue(TabStripBackgroundColorProperty, value);
    }

    public static readonly BindableProperty TabStripTextColorProperty = BindableProperty.Create(
        nameof(TabStripTextColor),
        typeof(Color),
        typeof(TabView),
        Colors.Black);

    public Color TabStripTextColor
    {
        get => (Color)GetValue(TabStripTextColorProperty);
        set => SetValue(TabStripTextColorProperty, value);
    }

    public static readonly BindableProperty SelectedTabColorProperty = BindableProperty.Create(
        nameof(SelectedTabColor),
        typeof(Color),
        typeof(TabView),
        Colors.Black);

    public Color SelectedTabColor
    {
        get => (Color)GetValue(SelectedTabColorProperty);
        set => SetValue(SelectedTabColorProperty, value);
    }

    public static readonly BindableProperty UnselectedTabColorProperty = BindableProperty.Create(
        nameof(UnselectedTabColor),
        typeof(Color),
        typeof(TabView),
        Colors.Gray);

    public Color UnselectedTabColor
    {
        get => (Color)GetValue(UnselectedTabColorProperty);
        set => SetValue(UnselectedTabColorProperty, value);
    }
}
