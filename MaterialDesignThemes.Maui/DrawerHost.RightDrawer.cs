namespace MaterialDesignThemes.Maui;

public sealed partial class DrawerHost
{
    private readonly ContentView _rightDrawerPresenter;

    public static readonly BindableProperty RightDrawerContentProperty =
        BindableProperty.Create(
            nameof(RightDrawerContent),
            typeof(View),
            typeof(DrawerHost),
            propertyChanged: OnRightDrawerContentChanged);

    public View? RightDrawerContent
    {
        get => (View?)GetValue(RightDrawerContentProperty);
        set => SetValue(RightDrawerContentProperty, value);
    }

    public static readonly BindableProperty RightDrawerWidthProperty =
        BindableProperty.Create(
            nameof(RightDrawerWidth),
            typeof(double),
            typeof(DrawerHost),
            DefaultDrawerSize,
            propertyChanged: OnRightDrawerWidthChanged);

    public double RightDrawerWidth
    {
        get => (double)GetValue(RightDrawerWidthProperty);
        set => SetValue(RightDrawerWidthProperty, value);
    }

    public static readonly BindableProperty IsRightDrawerOpenProperty =
        BindableProperty.Create(
            nameof(IsRightDrawerOpen),
            typeof(bool),
            typeof(DrawerHost),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsRightDrawerOpenChanged);

    public bool IsRightDrawerOpen
    {
        get => (bool)GetValue(IsRightDrawerOpenProperty);
        set => SetValue(IsRightDrawerOpenProperty, value);
    }

    public static readonly BindableProperty RightDrawerCloseOnClickAwayProperty =
        BindableProperty.Create(
            nameof(RightDrawerCloseOnClickAway),
            typeof(bool),
            typeof(DrawerHost),
            true,
            propertyChanged: OnRightDrawerCloseOnClickAwayChanged);

    public bool RightDrawerCloseOnClickAway
    {
        get => (bool)GetValue(RightDrawerCloseOnClickAwayProperty);
        set => SetValue(RightDrawerCloseOnClickAwayProperty, value);
    }

    private static void OnRightDrawerContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host._rightDrawerPresenter.Content = newValue as View;
        }
    }

    private static void OnRightDrawerWidthChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not DrawerHost host || newValue is not double width)
        {
            return;
        }

        host._rightDrawerPresenter.WidthRequest = width;

        if (host._layoutMode == DrawerLayoutMode.InlineRight && host._root.ColumnDefinitions.Count > 0)
        {
            host._root.ColumnDefinitions[0].Width = new GridLength(width, GridUnitType.Absolute);
        }
    }

    private static void OnRightDrawerCloseOnClickAwayChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host.UpdateOverlayVisibility();
        }
    }

    private static void OnIsRightDrawerOpenChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host && newValue is bool isOpen)
        {
            host.HandleDrawerStateChanged(DrawerDock.Right, isOpen);
        }
    }
}
