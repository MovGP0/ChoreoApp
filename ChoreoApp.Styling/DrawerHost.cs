namespace ChoreoApp.Styling;

/// <summary>
/// Responsive drawer host inspired by the WPF MaterialDesign DrawerHost.
/// Supports left, top, right and bottom drawers with optional overlay closing
/// and a responsive inline layout for the left drawer.
/// </summary>
public sealed partial class DrawerHost : ContentView
{
    private const double DefaultDrawerSize = 320d;
    private const double DefaultResponsiveBreakpoint = 900d;

    private readonly Grid _root;
    private readonly ContentView _mainPresenter;
    private readonly Grid _overlay;

    private bool _suppressStateHandler;
    private DrawerLayoutMode _layoutMode = DrawerLayoutMode.Overlay;

    public DrawerHost()
    {
        _mainPresenter = new ContentView();

        _leftDrawerPresenter = CreateDrawerPresenter();
        _topDrawerPresenter = CreateDrawerPresenter();
        _rightDrawerPresenter = CreateDrawerPresenter();
        _bottomDrawerPresenter = CreateDrawerPresenter();

        _leftDrawerPresenter.HorizontalOptions = LayoutOptions.Start;
        _leftDrawerPresenter.VerticalOptions = LayoutOptions.Fill;
        _leftDrawerPresenter.WidthRequest = DefaultDrawerSize;

        _rightDrawerPresenter.HorizontalOptions = LayoutOptions.End;
        _rightDrawerPresenter.VerticalOptions = LayoutOptions.Fill;
        _rightDrawerPresenter.WidthRequest = DefaultDrawerSize;

        _topDrawerPresenter.HorizontalOptions = LayoutOptions.Fill;
        _topDrawerPresenter.VerticalOptions = LayoutOptions.Start;
        _topDrawerPresenter.HeightRequest = DefaultDrawerSize;

        _bottomDrawerPresenter.HorizontalOptions = LayoutOptions.Fill;
        _bottomDrawerPresenter.VerticalOptions = LayoutOptions.End;
        _bottomDrawerPresenter.HeightRequest = DefaultDrawerSize;

        _overlay = new Grid
        {
            IsVisible = false,
            InputTransparent = false,
            Background = new SolidColorBrush(Color.FromArgb("#66000000"))
        };

        _overlay.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(OnOverlayTapped)
        });

        _root = new Grid
        {
            RowDefinitions = { new RowDefinition(GridLength.Star) }
        };

        _root.Children.Add(_mainPresenter);
        _root.Children.Add(_overlay);
        _root.Children.Add(_leftDrawerPresenter);
        _root.Children.Add(_topDrawerPresenter);
        _root.Children.Add(_rightDrawerPresenter);
        _root.Children.Add(_bottomDrawerPresenter);

        _mainPresenter.ZIndex = 0;
        _overlay.ZIndex = 1;
        _topDrawerPresenter.ZIndex = 2;
        _bottomDrawerPresenter.ZIndex = 3;
        _leftDrawerPresenter.ZIndex = 4;
        _rightDrawerPresenter.ZIndex = 5;

        Content = _root;

        SizeChanged += OnSizeChanged;
    }

    public event EventHandler<DrawerOpenedEventArgs>? DrawerOpened;

    public event EventHandler<DrawerClosingEventArgs>? DrawerClosing;

    public static readonly BindableProperty MainContentProperty =
        BindableProperty.Create(
            nameof(MainContent),
            typeof(View),
            typeof(DrawerHost),
            propertyChanged: OnMainContentChanged);

    public View? MainContent
    {
        get => (View?)GetValue(MainContentProperty);
        set => SetValue(MainContentProperty, value);
    }

    public static readonly BindableProperty OverlayBackgroundProperty =
        BindableProperty.Create(
            nameof(OverlayBackground),
            typeof(Brush),
            typeof(DrawerHost),
            new SolidColorBrush(Color.FromArgb("#66000000")),
            propertyChanged: OnOverlayBackgroundChanged);

    public Brush OverlayBackground
    {
        get => (Brush)GetValue(OverlayBackgroundProperty);
        set => SetValue(OverlayBackgroundProperty, value);
    }

    public static readonly BindableProperty ResponsiveBreakpointProperty =
        BindableProperty.Create(
            nameof(ResponsiveBreakpoint),
            typeof(double),
            typeof(DrawerHost),
            DefaultResponsiveBreakpoint,
            propertyChanged: OnResponsiveBreakpointChanged);

    public double ResponsiveBreakpoint
    {
        get => (double)GetValue(ResponsiveBreakpointProperty);
        set => SetValue(ResponsiveBreakpointProperty, value);
    }

    public static readonly BindableProperty OpenModeProperty =
        BindableProperty.Create(
            nameof(OpenMode),
            typeof(DrawerHostOpenMode),
            typeof(DrawerHost),
            DrawerHostOpenMode.Default,
            propertyChanged: OnOpenModeChanged);

    public DrawerHostOpenMode OpenMode
    {
        get => (DrawerHostOpenMode)GetValue(OpenModeProperty);
        set => SetValue(OpenModeProperty, value);
    }

    private ContentView CreateDrawerPresenter()
    {
        return new ContentView
        {
            IsVisible = false,
            InputTransparent = false,
            Background = new SolidColorBrush(Colors.White)
        };
    }

    private void OnOverlayTapped()
    {
        if (LeftDrawerCloseOnClickAway)
        {
            IsLeftDrawerOpen = false;
        }

        if (TopDrawerCloseOnClickAway)
        {
            IsTopDrawerOpen = false;
        }

        if (RightDrawerCloseOnClickAway)
        {
            IsRightDrawerOpen = false;
        }

        if (BottomDrawerCloseOnClickAway)
        {
            IsBottomDrawerOpen = false;
        }
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        UpdateLayoutMode(Width);
        UpdateAllDrawerVisualStates();
    }

    private void UpdateLayoutMode(double width)
    {
        var desiredMode = ShouldInlineLeftDrawer(width) ? DrawerLayoutMode.InlineLeft : DrawerLayoutMode.Overlay;
        if (_layoutMode == desiredMode)
        {
            return;
        }

        _layoutMode = desiredMode;

        _root.ColumnDefinitions.Clear();

        if (_layoutMode == DrawerLayoutMode.InlineLeft)
        {
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LeftDrawerWidth, GridUnitType.Absolute) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            Grid.SetColumn(_leftDrawerPresenter, 0);
            Grid.SetColumn(_mainPresenter, 1);
            Grid.SetColumnSpan(_overlay, 2);
            Grid.SetColumn(_topDrawerPresenter, 0);
            Grid.SetColumnSpan(_topDrawerPresenter, 2);
            Grid.SetColumn(_bottomDrawerPresenter, 0);
            Grid.SetColumnSpan(_bottomDrawerPresenter, 2);
            Grid.SetColumn(_rightDrawerPresenter, 1);
        }
        else
        {
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            Grid.SetColumn(_leftDrawerPresenter, 0);
            Grid.SetColumn(_mainPresenter, 0);
            Grid.SetColumnSpan(_overlay, 1);
            Grid.SetColumn(_topDrawerPresenter, 0);
            Grid.SetColumn(_bottomDrawerPresenter, 0);
            Grid.SetColumn(_rightDrawerPresenter, 0);
        }
    }

    private bool ShouldInlineLeftDrawer(double width)
    {
        if (OpenMode == DrawerHostOpenMode.Modal)
        {
            return false;
        }

        return width >= ResponsiveBreakpoint;
    }

    private void UpdateAllDrawerVisualStates()
    {
        UpdateDrawerVisualState(DrawerDock.Left, IsLeftDrawerOpen, _leftDrawerPresenter);
        UpdateDrawerVisualState(DrawerDock.Top, IsTopDrawerOpen, _topDrawerPresenter);
        UpdateDrawerVisualState(DrawerDock.Right, IsRightDrawerOpen, _rightDrawerPresenter);
        UpdateDrawerVisualState(DrawerDock.Bottom, IsBottomDrawerOpen, _bottomDrawerPresenter);
        UpdateOverlayVisibility();
    }

    private void UpdateOverlayVisibility()
    {
        bool anyOverlayDrawerOpen =
            (IsLeftDrawerOpen && _layoutMode == DrawerLayoutMode.Overlay && LeftDrawerCloseOnClickAway) ||
            (IsTopDrawerOpen && TopDrawerCloseOnClickAway) ||
            (IsRightDrawerOpen && RightDrawerCloseOnClickAway) ||
            (IsBottomDrawerOpen && BottomDrawerCloseOnClickAway);

        _overlay.IsVisible = anyOverlayDrawerOpen;
        _overlay.InputTransparent = !_overlay.IsVisible;
    }

    private void UpdateDrawerVisualState(DrawerDock dock, bool isOpen, ContentView presenter)
    {
        presenter.IsVisible = isOpen;
    }

    #region Property changed handlers

    private static void OnMainContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host._mainPresenter.Content = newValue as View;
        }
    }

    private static void OnOverlayBackgroundChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DrawerHost host)
        {
            host._overlay.Background = newValue as Brush ?? new SolidColorBrush(Color.FromArgb("#66000000"));
        }
    }

    private static void OnResponsiveBreakpointChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not DrawerHost host)
        {
            return;
        }

        host.UpdateLayoutMode(host.Width);
        host.UpdateAllDrawerVisualStates();
    }

    private static void OnOpenModeChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not DrawerHost host)
        {
            return;
        }

        host.UpdateLayoutMode(host.Width);
        host.UpdateAllDrawerVisualStates();
    }

    #endregion

    private void HandleDrawerStateChanged(DrawerDock dock, bool isOpen)
    {
        if (_suppressStateHandler)
        {
            return;
        }

        if (!isOpen)
        {
            var closingArgs = new DrawerClosingEventArgs(dock);
            DrawerClosing?.Invoke(this, closingArgs);

            if (closingArgs.IsCancelled)
            {
                _suppressStateHandler = true;
                SetDrawerOpenFlag(dock, true);
                _suppressStateHandler = false;
                return;
            }
        }

        var presenter = GetPresenter(dock);
        UpdateDrawerVisualState(dock, isOpen, presenter);
        UpdateOverlayVisibility();

        if (isOpen)
        {
            DrawerOpened?.Invoke(this, new DrawerOpenedEventArgs(dock));
        }
    }

    private ContentView GetPresenter(DrawerDock dock)
    {
        return dock switch
        {
            DrawerDock.Left => _leftDrawerPresenter,
            DrawerDock.Top => _topDrawerPresenter,
            DrawerDock.Right => _rightDrawerPresenter,
            DrawerDock.Bottom => _bottomDrawerPresenter,
            _ => _leftDrawerPresenter
        };
    }

    private void SetDrawerOpenFlag(DrawerDock dock, bool value)
    {
        switch (dock)
        {
            case DrawerDock.Left:
                SetValue(IsLeftDrawerOpenProperty, value);
                break;
            case DrawerDock.Top:
                SetValue(IsTopDrawerOpenProperty, value);
                break;
            case DrawerDock.Right:
                SetValue(IsRightDrawerOpenProperty, value);
                break;
            case DrawerDock.Bottom:
                SetValue(IsBottomDrawerOpenProperty, value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dock), dock, null);
        }
    }

    private enum DrawerLayoutMode
    {
        Overlay,
        InlineLeft,
        InlineRight
    }
}
