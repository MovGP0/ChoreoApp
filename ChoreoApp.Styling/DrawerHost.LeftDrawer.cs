namespace ChoreoApp.Styling;

public sealed partial class DrawerHost
{
    private readonly ContentView _leftDrawerPresenter;

    public static readonly BindableProperty LeftDrawerContentProperty =
        BindableProperty.Create(
            nameof(LeftDrawerContent),
            typeof(View),
            typeof(DrawerHost),
            propertyChanged: OnLeftDrawerContentChanged);

    public View? LeftDrawerContent
    {
        get => (View?)GetValue(LeftDrawerContentProperty);
        set => SetValue(LeftDrawerContentProperty, value);
    }

    public static readonly BindableProperty LeftDrawerWidthProperty =
        BindableProperty.Create(
            nameof(LeftDrawerWidth),
            typeof(double),
            typeof(DrawerHost),
            DefaultDrawerSize,
            propertyChanged: OnLeftDrawerWidthChanged);

    public double LeftDrawerWidth
    {
        get => (double)GetValue(LeftDrawerWidthProperty);
        set => SetValue(LeftDrawerWidthProperty, value);
    }

    public static readonly BindableProperty IsLeftDrawerOpenProperty =
        BindableProperty.Create(
            nameof(IsLeftDrawerOpen),
            typeof(bool),
            typeof(DrawerHost),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsLeftDrawerOpenChanged);

    public bool IsLeftDrawerOpen
    {
        get => (bool)GetValue(IsLeftDrawerOpenProperty);
        set => SetValue(IsLeftDrawerOpenProperty, value);
    }

    public static readonly BindableProperty LeftDrawerCloseOnClickAwayProperty =
        BindableProperty.Create(
            nameof(LeftDrawerCloseOnClickAway),
            typeof(bool),
            typeof(DrawerHost),
            true,
            propertyChanged: OnLeftDrawerCloseOnClickAwayChanged);

    public bool LeftDrawerCloseOnClickAway
    {
        get => (bool)GetValue(LeftDrawerCloseOnClickAwayProperty);
        set => SetValue(LeftDrawerCloseOnClickAwayProperty, value);
    }

    private static void OnLeftDrawerContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host._leftDrawerPresenter.Content = newValue as View;
        }
    }

    private static void OnLeftDrawerWidthChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not DrawerHost host || newValue is not double width)
        {
            return;
        }

        host._leftDrawerPresenter.WidthRequest = width;

        if (host._layoutMode == DrawerLayoutMode.InlineLeft && host._root.ColumnDefinitions.Count > 0)
        {
            host._root.ColumnDefinitions[0].Width = new GridLength(width, GridUnitType.Absolute);
        }
    }

    private static void OnLeftDrawerCloseOnClickAwayChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host.UpdateOverlayVisibility();
        }
    }

    private static void OnIsLeftDrawerOpenChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host && newValue is bool isOpen)
        {
            host.HandleDrawerStateChanged(DrawerDock.Left, isOpen);
        }
    }
}
