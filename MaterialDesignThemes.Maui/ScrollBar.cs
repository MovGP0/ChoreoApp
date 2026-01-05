namespace MaterialDesignThemes.Maui;

public sealed class ScrollBar : TemplatedView
{
    public const string TrackLayoutPartName = "PART_TrackLayout";
    public const string ThumbPartName = "PART_Thumb";
    public const string DecreaseButtonPartName = "PART_DecreaseButton";
    public const string IncreaseButtonPartName = "PART_IncreaseButton";

    private AbsoluteLayout? _trackLayout;
    private View? _thumb;
    private View? _decreaseButton;
    private View? _increaseButton;
    private double _dragStartValue;
    private bool _isDragging;

    public event EventHandler<ValueChangedEventArgs<double>>? ValueChanged;

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(ScrollBar),
        StackOrientation.Vertical,
        propertyChanged: OnLayoutPropertyChanged);

    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
        nameof(Minimum),
        typeof(double),
        typeof(ScrollBar),
        0d,
        propertyChanged: OnLayoutPropertyChanged);

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
        nameof(Maximum),
        typeof(double),
        typeof(ScrollBar),
        1d,
        propertyChanged: OnLayoutPropertyChanged);

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(double),
        typeof(ScrollBar),
        0d,
        BindingMode.TwoWay,
        propertyChanged: OnValueChanged);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty ViewportSizeProperty = BindableProperty.Create(
        nameof(ViewportSize),
        typeof(double),
        typeof(ScrollBar),
        0d,
        propertyChanged: OnLayoutPropertyChanged);

    public double ViewportSize
    {
        get => (double)GetValue(ViewportSizeProperty);
        set => SetValue(ViewportSizeProperty, value);
    }

    public static readonly BindableProperty SmallChangeProperty = BindableProperty.Create(
        nameof(SmallChange),
        typeof(double),
        typeof(ScrollBar),
        16d);

    public double SmallChange
    {
        get => (double)GetValue(SmallChangeProperty);
        set => SetValue(SmallChangeProperty, value);
    }

    public static readonly BindableProperty LargeChangeProperty = BindableProperty.Create(
        nameof(LargeChange),
        typeof(double),
        typeof(ScrollBar),
        48d);

    public double LargeChange
    {
        get => (double)GetValue(LargeChangeProperty);
        set => SetValue(LargeChangeProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _trackLayout = GetTemplateChild(TrackLayoutPartName) as AbsoluteLayout;
        _thumb = GetTemplateChild(ThumbPartName) as View;
        _decreaseButton = GetTemplateChild(DecreaseButtonPartName) as View;
        _increaseButton = GetTemplateChild(IncreaseButtonPartName) as View;

        if (_trackLayout is not null)
        {
            _trackLayout.SizeChanged += OnTrackLayoutSizeChanged;
            var tap = new TapGestureRecognizer();
            tap.Tapped += OnTrackTapped;
            _trackLayout.GestureRecognizers.Add(tap);
        }

        if (_thumb is not null)
        {
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnThumbPanUpdated;
            _thumb.GestureRecognizers.Add(pan);
        }

        HookButton(_decreaseButton, OnDecreaseClicked);
        HookButton(_increaseButton, OnIncreaseClicked);

        UpdateThumbLayout();
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ScrollBar bar)
        {
            return;
        }

        bar.UpdateThumbLayout();
        bar.ValueChanged?.Invoke(bar, new ValueChangedEventArgs<double>((double)oldValue, (double)newValue));
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ScrollBar bar)
        {
            bar.UpdateThumbLayout();
        }
    }

    private void OnTrackLayoutSizeChanged(object? sender, EventArgs e)
    {
        UpdateThumbLayout();
    }

    private void OnTrackTapped(object? sender, TappedEventArgs e)
    {
        if (_trackLayout is null)
        {
            return;
        }

        var point = e.GetPosition(_trackLayout);
        if (point is null)
        {
            return;
        }

        var ratio = Orientation == StackOrientation.Horizontal
            ? point.Value.X / Math.Max(1, _trackLayout.Width)
            : point.Value.Y / Math.Max(1, _trackLayout.Height);
        Value = Minimum + (Maximum - Minimum) * ratio.Clamp01();
    }

    private void OnThumbPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_trackLayout is null)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _isDragging = true;
                _dragStartValue = Value;
                break;
            case GestureStatus.Running:
                var delta = Orientation == StackOrientation.Horizontal ? e.TotalX : e.TotalY;
                var length = Orientation == StackOrientation.Horizontal ? _trackLayout.Width : _trackLayout.Height;
                var range = Math.Max(0, Maximum - Minimum);
                if (range <= 0 || length <= 0)
                {
                    return;
                }

                var ratio = delta / Math.Max(1, length);
                Value = (_dragStartValue + range * ratio).Clamp(Minimum, Maximum);
                break;
            default:
                _isDragging = false;
                break;
        }
    }

    private void OnDecreaseClicked(object? sender, EventArgs e)
    {
        Value = (Value - SmallChange).Clamp(Minimum, Maximum);
    }

    private void OnIncreaseClicked(object? sender, EventArgs e)
    {
        Value = (Value + SmallChange).Clamp(Minimum, Maximum);
    }

    private void UpdateThumbLayout()
    {
        if (_trackLayout is null || _thumb is null)
        {
            return;
        }

        var range = Math.Max(0, Maximum - Minimum);
        var length = Orientation == StackOrientation.Horizontal ? _trackLayout.Width : _trackLayout.Height;
        if (length <= 0)
        {
            return;
        }

        var thumbLength = ViewportSize > 0 && range > 0
            ? Math.Max(10, length * (ViewportSize / (ViewportSize + range)))
            : Math.Max(10, length * 0.2);

        var ratio = range <= 0 ? 0 : (Value - Minimum) / range;
        var pos = (length - thumbLength) * ratio.Clamp01();

        if (Orientation == StackOrientation.Horizontal)
        {
            AbsoluteLayout.SetLayoutBounds(_thumb, new Rect(pos, 0, thumbLength, _trackLayout.Height));
        }
        else
        {
            AbsoluteLayout.SetLayoutBounds(_thumb, new Rect(0, pos, _trackLayout.Width, thumbLength));
        }
    }

    private static void HookButton(View? view, EventHandler handler)
    {
        switch (view)
        {
            case Button button:
                button.Clicked += handler;
                break;
            case ContentButton contentButton:
                contentButton.Clicked += handler;
                break;
        }
    }
}
