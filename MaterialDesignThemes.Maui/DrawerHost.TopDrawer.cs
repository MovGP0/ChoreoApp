namespace MaterialDesignThemes.Maui;

public sealed partial class DrawerHost
{
    private readonly ContentView _topDrawerPresenter;

    public static readonly BindableProperty TopDrawerContentProperty =
        BindableProperty.Create(
            nameof(TopDrawerContent),
            typeof(View),
            typeof(DrawerHost),
            propertyChanged: OnTopDrawerContentChanged);

    public View? TopDrawerContent
    {
        get => (View?)GetValue(TopDrawerContentProperty);
        set => SetValue(TopDrawerContentProperty, value);
    }

    public static readonly BindableProperty TopDrawerHeightProperty =
        BindableProperty.Create(
            nameof(TopDrawerHeight),
            typeof(double),
            typeof(DrawerHost),
            DefaultDrawerSize,
            propertyChanged: OnTopDrawerHeightChanged);

    public double TopDrawerHeight
    {
        get => (double)GetValue(TopDrawerHeightProperty);
        set => SetValue(TopDrawerHeightProperty, value);
    }

    public static readonly BindableProperty IsTopDrawerOpenProperty =
        BindableProperty.Create(
            nameof(IsTopDrawerOpen),
            typeof(bool),
            typeof(DrawerHost),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsTopDrawerOpenChanged);

    public bool IsTopDrawerOpen
    {
        get => (bool)GetValue(IsTopDrawerOpenProperty);
        set => SetValue(IsTopDrawerOpenProperty, value);
    }

    public static readonly BindableProperty TopDrawerCloseOnClickAwayProperty =
        BindableProperty.Create(
            nameof(TopDrawerCloseOnClickAway),
            typeof(bool),
            typeof(DrawerHost),
            true,
            propertyChanged: OnTopDrawerCloseOnClickAwayChanged);

    public bool TopDrawerCloseOnClickAway
    {
        get => (bool)GetValue(TopDrawerCloseOnClickAwayProperty);
        set => SetValue(TopDrawerCloseOnClickAwayProperty, value);
    }

    private static void OnTopDrawerContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host._topDrawerPresenter.Content = newValue as View;
        }
    }

    private static void OnTopDrawerHeightChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host && newValue is double height)
        {
            host._topDrawerPresenter.HeightRequest = height;
        }
    }

    private static void OnTopDrawerCloseOnClickAwayChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host.UpdateOverlayVisibility();
        }
    }

    private static void OnIsTopDrawerOpenChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host && newValue is bool isOpen)
        {
            host.HandleDrawerStateChanged(DrawerDock.Top, isOpen);
        }
    }
}
