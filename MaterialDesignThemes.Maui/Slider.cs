namespace MaterialDesignThemes.Maui;

public sealed class Slider : TemplatedView
{
    public const string TrackLayoutPartName = "PART_TrackLayout";
    public const string TrackPartName = "PART_Track";
    public const string ActiveTrackPartName = "PART_ActiveTrack";
    public const string ThumbPartName = "PART_Thumb";
    public const string ValueLabelPartName = "PART_ValueLabel";
    public const string ThumbImagePartName = "PART_ThumbImage";

    private AbsoluteLayout? _trackLayout;
    private View? _track;
    private View? _activeTrack;
    private View? _thumb;
    private Label? _valueLabel;
    private Image? _thumbImage;
    private double _dragStartValue;

    public event EventHandler<ValueChangedEventArgs<double>>? ValueChanged;
    public event EventHandler? DragStarted;
    public event EventHandler? DragCompleted;

    public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
        nameof(Minimum),
        typeof(double),
        typeof(Slider),
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
        typeof(Slider),
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
        typeof(Slider),
        0d,
        BindingMode.TwoWay,
        propertyChanged: OnValueChanged);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(Slider),
        StackOrientation.Horizontal,
        propertyChanged: OnLayoutPropertyChanged);

    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly BindableProperty TickFrequencyProperty = BindableProperty.Create(
        nameof(TickFrequency),
        typeof(double),
        typeof(Slider),
        1d);

    public double TickFrequency
    {
        get => (double)GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    public static readonly BindableProperty IsSnapToTickEnabledProperty = BindableProperty.Create(
        nameof(IsSnapToTickEnabled),
        typeof(bool),
        typeof(Slider),
        false);

    public bool IsSnapToTickEnabled
    {
        get => (bool)GetValue(IsSnapToTickEnabledProperty);
        set => SetValue(IsSnapToTickEnabledProperty, value);
    }

    public static readonly BindableProperty TrackThicknessProperty = BindableProperty.Create(
        nameof(TrackThickness),
        typeof(double),
        typeof(Slider),
        4d,
        propertyChanged: OnLayoutPropertyChanged);

    public double TrackThickness
    {
        get => (double)GetValue(TrackThicknessProperty);
        set => SetValue(TrackThicknessProperty, value);
    }

    public static readonly BindableProperty ThumbSizeProperty = BindableProperty.Create(
        nameof(ThumbSize),
        typeof(double),
        typeof(Slider),
        20d,
        propertyChanged: OnLayoutPropertyChanged);

    public double ThumbSize
    {
        get => (double)GetValue(ThumbSizeProperty);
        set => SetValue(ThumbSizeProperty, value);
    }

    public static readonly BindableProperty ShowValueLabelProperty = BindableProperty.Create(
        nameof(ShowValueLabel),
        typeof(bool),
        typeof(Slider),
        false,
        propertyChanged: OnLayoutPropertyChanged);

    public bool ShowValueLabel
    {
        get => (bool)GetValue(ShowValueLabelProperty);
        set => SetValue(ShowValueLabelProperty, value);
    }

    public static readonly BindableProperty MinimumTrackColorProperty = BindableProperty.Create(
        nameof(MinimumTrackColor),
        typeof(Color),
        typeof(Slider),
        null,
        propertyChanged: OnAppearancePropertyChanged);

    public Color? MinimumTrackColor
    {
        get => (Color?)GetValue(MinimumTrackColorProperty);
        set => SetValue(MinimumTrackColorProperty, value);
    }

    public static readonly BindableProperty MaximumTrackColorProperty = BindableProperty.Create(
        nameof(MaximumTrackColor),
        typeof(Color),
        typeof(Slider),
        null,
        propertyChanged: OnAppearancePropertyChanged);

    public Color? MaximumTrackColor
    {
        get => (Color?)GetValue(MaximumTrackColorProperty);
        set => SetValue(MaximumTrackColorProperty, value);
    }

    public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(
        nameof(ThumbColor),
        typeof(Color),
        typeof(Slider),
        null,
        propertyChanged: OnAppearancePropertyChanged);

    public Color? ThumbColor
    {
        get => (Color?)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public static readonly BindableProperty ThumbImageSourceProperty = BindableProperty.Create(
        nameof(ThumbImageSource),
        typeof(ImageSource),
        typeof(Slider),
        null,
        propertyChanged: OnAppearancePropertyChanged);

    public ImageSource? ThumbImageSource
    {
        get => (ImageSource?)GetValue(ThumbImageSourceProperty);
        set => SetValue(ThumbImageSourceProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _trackLayout = GetTemplateChild(TrackLayoutPartName) as AbsoluteLayout;
        _track = GetTemplateChild(TrackPartName) as View;
        _activeTrack = GetTemplateChild(ActiveTrackPartName) as View;
        _thumb = GetTemplateChild(ThumbPartName) as View;
        _valueLabel = GetTemplateChild(ValueLabelPartName) as Label;
        _thumbImage = GetTemplateChild(ThumbImagePartName) as Image;

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

        UpdateLayout();
        UpdateValueLabel();
        ApplyAppearance();
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Slider slider)
        {
            return;
        }

        slider.UpdateLayout();
        slider.UpdateValueLabel();
        slider.ValueChanged?.Invoke(slider, new ValueChangedEventArgs<double>((double)oldValue, (double)newValue));
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Slider slider)
        {
            slider.UpdateLayout();
        }
    }

    private static void OnAppearancePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Slider slider)
        {
            slider.ApplyAppearance();
        }
    }

    private void OnTrackLayoutSizeChanged(object? sender, EventArgs e)
    {
        UpdateLayout();
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
        SetValueFromRatio(Clamp01(ratio));
        if (SliderAssist.GetFocusSliderOnClick(this))
        {
            Focus();
        }
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
                _dragStartValue = Value;
                DragStarted?.Invoke(this, EventArgs.Empty);
                break;
            case GestureStatus.Running:
                var delta = Orientation == StackOrientation.Horizontal ? e.TotalX : e.TotalY;
                var length = Orientation == StackOrientation.Horizontal ? _trackLayout.Width : _trackLayout.Height;
                var range = Math.Max(0, Maximum - Minimum);
                if (length <= 0 || range <= 0)
                {
                    return;
                }

                var ratio = delta / Math.Max(1, length - ThumbSize);
                SetValueFromRatio(Clamp01((_dragStartValue - Minimum) / range + ratio));
                break;
            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                DragCompleted?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void SetValueFromRatio(double ratio)
    {
        var range = Math.Max(0, Maximum - Minimum);
        var value = Minimum + range * ratio;
        Value = IsSnapToTickEnabled ? SnapToTick(value) : value;
    }

    private double SnapToTick(double value)
    {
        if (TickFrequency <= 0)
        {
            return value;
        }

        var relative = value - Minimum;
        var snapped = Math.Round(relative / TickFrequency) * TickFrequency + Minimum;
        return Clamp(snapped, Minimum, Maximum);
    }

    private void UpdateLayout()
    {
        if (_trackLayout is null || _track is null || _activeTrack is null || _thumb is null)
        {
            return;
        }

        var range = Math.Max(0, Maximum - Minimum);
        var ratio = range <= 0 ? 0 : (Value - Minimum) / range;
        ratio = Clamp01(ratio);

        var length = Orientation == StackOrientation.Horizontal ? _trackLayout.Width : _trackLayout.Height;
        if (length <= 0)
        {
            return;
        }

        var position = (length - ThumbSize) * ratio;

        if (Orientation == StackOrientation.Horizontal)
        {
            _track.HeightRequest = TrackThickness;
            _activeTrack.HeightRequest = TrackThickness;
            _track.WidthRequest = _trackLayout.Width;
            _activeTrack.WidthRequest = position + ThumbSize / 2;
            AbsoluteLayout.SetLayoutBounds(_track, new Rect(0, (_trackLayout.Height - TrackThickness) / 2, _trackLayout.Width, TrackThickness));
            AbsoluteLayout.SetLayoutBounds(_activeTrack, new Rect(0, (_trackLayout.Height - TrackThickness) / 2, position + ThumbSize / 2, TrackThickness));
            AbsoluteLayout.SetLayoutBounds(_thumb, new Rect(position, (_trackLayout.Height - ThumbSize) / 2, ThumbSize, ThumbSize));
        }
        else
        {
            _track.WidthRequest = TrackThickness;
            _activeTrack.WidthRequest = TrackThickness;
            _track.HeightRequest = _trackLayout.Height;
            _activeTrack.HeightRequest = position + ThumbSize / 2;
            AbsoluteLayout.SetLayoutBounds(_track, new Rect((_trackLayout.Width - TrackThickness) / 2, 0, TrackThickness, _trackLayout.Height));
            AbsoluteLayout.SetLayoutBounds(_activeTrack, new Rect((_trackLayout.Width - TrackThickness) / 2, 0, TrackThickness, position + ThumbSize / 2));
            AbsoluteLayout.SetLayoutBounds(_thumb, new Rect((_trackLayout.Width - ThumbSize) / 2, position, ThumbSize, ThumbSize));
        }

        if (_valueLabel is not null)
        {
            _valueLabel.IsVisible = ShowValueLabel;
        }
    }

    private void UpdateValueLabel()
    {
        if (_valueLabel is null)
        {
            return;
        }

        var format = SliderAssist.GetToolTipFormat(this);
        _valueLabel.Text = string.IsNullOrWhiteSpace(format)
            ? Value.ToString("0.##", System.Globalization.CultureInfo.CurrentCulture)
            : string.Format(System.Globalization.CultureInfo.CurrentCulture, format, Value);
    }

    private void ApplyAppearance()
    {
        if (_track is BoxView track && MaximumTrackColor is not null)
        {
            track.Color = MaximumTrackColor;
        }

        if (_activeTrack is BoxView activeTrack && MinimumTrackColor is not null)
        {
            activeTrack.Color = MinimumTrackColor;
        }

        if (_thumb is Border thumb && ThumbColor is not null)
        {
            thumb.BackgroundColor = ThumbColor;
        }

        if (_thumbImage is not null)
        {
            _thumbImage.Source = ThumbImageSource;
            _thumbImage.IsVisible = ThumbImageSource is not null;
        }
    }

    private static double Clamp(double value, double min, double max)
        => Math.Min(Math.Max(value, min), max);

    private static double Clamp01(double value)
        => Math.Min(Math.Max(value, 0), 1);
}
