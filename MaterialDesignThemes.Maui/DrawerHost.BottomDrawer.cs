namespace MaterialDesignThemes.Maui;

public sealed partial class DrawerHost
{
private readonly ContentView _bottomDrawerPresenter;

    public static readonly BindableProperty BottomDrawerContentProperty =
        BindableProperty.Create(
            nameof(BottomDrawerContent),
            typeof(View),
            typeof(DrawerHost),
            propertyChanged: OnBottomDrawerContentChanged);

    public View? BottomDrawerContent
    {
        get => (View?)GetValue(BottomDrawerContentProperty);
        set => SetValue(BottomDrawerContentProperty, value);
    }

    public static readonly BindableProperty BottomDrawerHeightProperty =
        BindableProperty.Create(
            nameof(BottomDrawerHeight),
            typeof(double),
            typeof(DrawerHost),
            DefaultDrawerSize,
            propertyChanged: OnBottomDrawerHeightChanged);

    public double BottomDrawerHeight
    {
        get => (double)GetValue(BottomDrawerHeightProperty);
        set => SetValue(BottomDrawerHeightProperty, value);
    }

    public static readonly BindableProperty IsBottomDrawerOpenProperty =
        BindableProperty.Create(
            nameof(IsBottomDrawerOpen),
            typeof(bool),
            typeof(DrawerHost),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsBottomDrawerOpenChanged);

    public bool IsBottomDrawerOpen
    {
        get => (bool)GetValue(IsBottomDrawerOpenProperty);
        set => SetValue(IsBottomDrawerOpenProperty, value);
    }

    public static readonly BindableProperty BottomDrawerCloseOnClickAwayProperty =
        BindableProperty.Create(
            nameof(BottomDrawerCloseOnClickAway),
            typeof(bool),
            typeof(DrawerHost),
            true,
            propertyChanged: OnBottomDrawerCloseOnClickAwayChanged);

    public bool BottomDrawerCloseOnClickAway
    {
        get => (bool)GetValue(BottomDrawerCloseOnClickAwayProperty);
        set => SetValue(BottomDrawerCloseOnClickAwayProperty, value);
    }

    private static void OnBottomDrawerContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host._bottomDrawerPresenter.Content = newValue as View;
        }
    }

    private static void OnBottomDrawerHeightChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host && newValue is double height)
        {
            host._bottomDrawerPresenter.HeightRequest = height;
        }
    }

    private static void OnBottomDrawerCloseOnClickAwayChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host.UpdateOverlayVisibility();
        }
    }

    private static void OnIsBottomDrawerOpenChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host && newValue is bool isOpen)
        {
            host.HandleDrawerStateChanged(DrawerDock.Bottom, isOpen);
        }
    }
}
