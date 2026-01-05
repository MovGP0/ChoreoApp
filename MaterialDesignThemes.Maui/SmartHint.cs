namespace MaterialDesignThemes.Maui;

/// <summary>
/// Placeholder/floating hint control for MAUI. Mirrors the public surface of WPF SmartHint.
/// </summary>
public class SmartHint : ContentView
{
    public const string ContentStatesGroupName = "ContentStates";
    public const string HintRestingPositionName = "HintRestingPosition";
    public const string HintFloatingPositionName = "HintFloatingPosition";

    public static readonly BindableProperty HintProxyProperty =
        BindableProperty.Create(
            nameof(HintProxy),
            typeof(IHintProxy),
            typeof(SmartHint),
            propertyChanged: OnHintProxyChanged);

    public IHintProxy? HintProxy
    {
        get => (IHintProxy?)GetValue(HintProxyProperty);
        set => SetValue(HintProxyProperty, value);
    }

    public static readonly BindableProperty HintProperty =
        BindableProperty.Create(
            nameof(Hint),
            typeof(object),
            typeof(SmartHint));

    public object? Hint
    {
        get => GetValue(HintProperty);
        set => SetValue(HintProperty, value);
    }

    private static readonly BindablePropertyKey IsContentNullOrEmptyPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(IsContentNullOrEmpty),
            typeof(bool),
            typeof(SmartHint),
            true);

    public static readonly BindableProperty IsContentNullOrEmptyProperty =
        IsContentNullOrEmptyPropertyKey.BindableProperty;

    public bool IsContentNullOrEmpty
    {
        get => (bool)GetValue(IsContentNullOrEmptyProperty);
        private set => SetValue(IsContentNullOrEmptyPropertyKey, value);
    }

    private static readonly BindablePropertyKey IsHintInFloatingPositionPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(IsHintInFloatingPosition),
            typeof(bool),
            typeof(SmartHint),
            false);

    public static readonly BindableProperty IsHintInFloatingPositionProperty =
        IsHintInFloatingPositionPropertyKey.BindableProperty;

    public bool IsHintInFloatingPosition
    {
        get => (bool)GetValue(IsHintInFloatingPositionProperty);
        private set => SetValue(IsHintInFloatingPositionPropertyKey, value);
    }

    public static readonly BindableProperty UseFloatingProperty =
        BindableProperty.Create(
            nameof(UseFloating),
            typeof(bool),
            typeof(SmartHint),
            false);

    public bool UseFloating
    {
        get => (bool)GetValue(UseFloatingProperty);
        set => SetValue(UseFloatingProperty, value);
    }

    public static readonly BindableProperty FloatingScaleProperty =
        BindableProperty.Create(
            nameof(FloatingScale),
            typeof(double),
            typeof(SmartHint),
            0.74d);

    public double FloatingScale
    {
        get => (double)GetValue(FloatingScaleProperty);
        set => SetValue(FloatingScaleProperty, value);
    }

    public static readonly BindableProperty FloatingOffsetProperty =
        BindableProperty.Create(
            nameof(FloatingOffset),
            typeof(Point),
            typeof(SmartHint),
            new Point(0, 0));

    public Point FloatingOffset
    {
        get => (Point)GetValue(FloatingOffsetProperty);
        set => SetValue(FloatingOffsetProperty, value);
    }

    public static readonly BindableProperty HintOpacityProperty =
        BindableProperty.Create(
            nameof(HintOpacity),
            typeof(double),
            typeof(SmartHint),
            0.46d);

    public double HintOpacity
    {
        get => (double)GetValue(HintOpacityProperty);
        set => SetValue(HintOpacityProperty, value);
    }

    public static readonly BindableProperty InitialVerticalOffsetProperty =
        BindableProperty.Create(
            nameof(InitialVerticalOffset),
            typeof(double),
            typeof(SmartHint),
            0d);

    public double InitialVerticalOffset
    {
        get => (double)GetValue(InitialVerticalOffsetProperty);
        set => SetValue(InitialVerticalOffsetProperty, value);
    }

    public static readonly BindableProperty InitialHorizontalOffsetProperty =
        BindableProperty.Create(
            nameof(InitialHorizontalOffset),
            typeof(double),
            typeof(SmartHint),
            0d);

    public double InitialHorizontalOffset
    {
        get => (double)GetValue(InitialHorizontalOffsetProperty);
        set => SetValue(InitialHorizontalOffsetProperty, value);
    }

    public static readonly BindableProperty FloatingTargetProperty =
        BindableProperty.Create(
            nameof(FloatingTarget),
            typeof(View),
            typeof(SmartHint));

    public View? FloatingTarget
    {
        get => (View?)GetValue(FloatingTargetProperty);
        set => SetValue(FloatingTargetProperty, value);
    }

    public static readonly BindableProperty HintHostProperty =
        BindableProperty.Create(
            nameof(HintHost),
            typeof(View),
            typeof(SmartHint));

    public View? HintHost
    {
        get => (View?)GetValue(HintHostProperty);
        set => SetValue(HintHostProperty, value);
    }

    public static readonly BindableProperty FloatingAlignmentProperty =
        BindableProperty.Create(
            nameof(FloatingAlignment),
            typeof(LayoutAlignment),
            typeof(SmartHint),
            LayoutAlignment.End);

    public LayoutAlignment FloatingAlignment
    {
        get => (LayoutAlignment)GetValue(FloatingAlignmentProperty);
        set => SetValue(FloatingAlignmentProperty, value);
    }

    public static readonly BindableProperty FloatingMarginProperty =
        BindableProperty.Create(
            nameof(FloatingMargin),
            typeof(Thickness),
            typeof(SmartHint),
            new Thickness(0));

    public Thickness FloatingMargin
    {
        get => (Thickness)GetValue(FloatingMarginProperty);
        set => SetValue(FloatingMarginProperty, value);
    }

    private static void OnHintProxyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SmartHint hint)
        {
            return;
        }

        if (oldValue is IHintProxy oldProxy)
        {
            oldProxy.IsVisibleChanged -= hint.OnHintProxyIsVisibleChanged;
            oldProxy.ContentChanged -= hint.OnHintProxyContentChanged;
            oldProxy.Loaded -= hint.OnHintProxyContentChanged;
            oldProxy.FocusedChanged -= hint.OnHintProxyFocusedChanged;
            oldProxy.Dispose();
        }

        if (newValue is IHintProxy newProxy)
        {
            newProxy.IsVisibleChanged += hint.OnHintProxyIsVisibleChanged;
            newProxy.ContentChanged += hint.OnHintProxyContentChanged;
            newProxy.Loaded += hint.OnHintProxyContentChanged;
            newProxy.FocusedChanged += hint.OnHintProxyFocusedChanged;
            hint.RefreshState(false);
        }
    }

    protected virtual void OnHintProxyFocusedChanged(object? sender, EventArgs e)
    {
        if (HintProxy is { IsLoaded: true })
        {
            RefreshState(true);
        }
        else if (HintProxy is { } proxy)
        {
            proxy.Loaded += HintProxySetStateOnLoaded;
        }
    }

    protected virtual void OnHintProxyContentChanged(object? sender, EventArgs e)
    {
        IsContentNullOrEmpty = HintProxy?.IsEmpty() == true;

        if (HintProxy is { IsLoaded: true })
        {
            RefreshState(true);
        }
        else if (HintProxy is { } proxy)
        {
            proxy.Loaded += HintProxySetStateOnLoaded;
        }
    }

    private void HintProxySetStateOnLoaded(object? sender, EventArgs e)
    {
        RefreshState(false);
        if (HintProxy is { } proxy)
        {
            proxy.Loaded -= HintProxySetStateOnLoaded;
        }
    }

    protected virtual void OnHintProxyIsVisibleChanged(object? sender, EventArgs e) =>
        RefreshState(false);

    private void RefreshState(bool useTransitions)
    {
        var proxy = HintProxy;
        if (proxy == null || !proxy.IsVisible)
        {
            return;
        }

        IsContentNullOrEmpty = proxy.IsEmpty();

        void Update()
        {
            bool isEmpty = proxy.IsEmpty();
            bool isFocused = HintHost?.IsFocused == true || proxy.IsFocused;

            string state;
            if (UseFloating)
            {
                state = !isEmpty || isFocused ? HintFloatingPositionName : HintRestingPositionName;
            }
            else
            {
                state = !isEmpty ? HintFloatingPositionName : HintRestingPositionName;
            }

            IsHintInFloatingPosition = state == HintFloatingPositionName;
            VisualStateManager.GoToState(this, state);
        }

        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(Update);
        }
        else
        {
            Update();
        }
    }
}
