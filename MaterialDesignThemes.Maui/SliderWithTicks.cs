using System.Globalization;
using System.Windows.Input;

namespace MaterialDesignThemes.Maui;

public sealed class SliderWithTicks : ContentView
{
    private readonly Slider _slider = new();
    private readonly GraphicsView _ticksView;
    private readonly SliderWithTicksTickDrawable _tickDrawable = new();
    private bool _isUpdatingFromSlider;

    public SliderWithTicks()
    {
        _ticksView = new GraphicsView
        {
            HeightRequest = 6,
            Drawable = _tickDrawable
        };

        _slider.ValueChanged += OnSliderValueChanged;
        _slider.DragStarted += OnSliderDragStarted;
        _slider.DragCompleted += OnSliderDragCompleted;

        _slider.SetBinding(IsEnabledProperty, new Binding(nameof(IsEnabled), source: this));

        var layout = new Grid
        {
            RowDefinitions =
            [
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            ]
        };

        layout.Add(_slider);
        layout.Add(_ticksView, 0, 1);

        Content = layout;

        UpdateSliderProperties();
        UpdateTickDrawable();
    }

    public event EventHandler<ValueChangedEventArgs>? ValueChanged;
    public event EventHandler? DragStarted;
    public event EventHandler? DragCompleted;

    public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
        nameof(Minimum),
        typeof(double),
        typeof(SliderWithTicks),
        0d,
        coerceValue: (bindable, value) =>
        {
            var slider = (SliderWithTicks)bindable;
            slider.Value = Math.Clamp(slider.Value, (double)value, slider.Maximum);
            return value;
        },
        propertyChanged: OnRangeChanged);

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
        nameof(Maximum),
        typeof(double),
        typeof(SliderWithTicks),
        1d,
        coerceValue: (bindable, value) =>
        {
            var slider = (SliderWithTicks)bindable;
            slider.Value = Math.Clamp(slider.Value, slider.Minimum, (double)value);
            return value;
        },
        propertyChanged: OnRangeChanged);

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(double),
        typeof(SliderWithTicks),
        0d,
        BindingMode.TwoWay,
        coerceValue: (bindable, value) =>
        {
            var slider = (SliderWithTicks)bindable;
            return Math.Clamp((double)value, slider.Minimum, slider.Maximum);
        },
        propertyChanged: OnValueChanged);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty MinimumTrackColorProperty = BindableProperty.Create(
        nameof(MinimumTrackColor),
        typeof(Color),
        typeof(SliderWithTicks),
        propertyChanged: OnAppearanceChanged);

    public Color? MinimumTrackColor
    {
        get => (Color?)GetValue(MinimumTrackColorProperty);
        set => SetValue(MinimumTrackColorProperty, value);
    }

    public static readonly BindableProperty MaximumTrackColorProperty = BindableProperty.Create(
        nameof(MaximumTrackColor),
        typeof(Color),
        typeof(SliderWithTicks),
        propertyChanged: OnAppearanceChanged);

    public Color? MaximumTrackColor
    {
        get => (Color?)GetValue(MaximumTrackColorProperty);
        set => SetValue(MaximumTrackColorProperty, value);
    }

    public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(
        nameof(ThumbColor),
        typeof(Color),
        typeof(SliderWithTicks),
        propertyChanged: OnAppearanceChanged);

    public Color? ThumbColor
    {
        get => (Color?)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public static readonly BindableProperty ThumbImageSourceProperty = BindableProperty.Create(
        nameof(ThumbImageSource),
        typeof(ImageSource),
        typeof(SliderWithTicks),
        propertyChanged: OnAppearanceChanged);

    public ImageSource? ThumbImageSource
    {
        get => (ImageSource?)GetValue(ThumbImageSourceProperty);
        set => SetValue(ThumbImageSourceProperty, value);
    }

    public static readonly BindableProperty DragStartedCommandProperty = BindableProperty.Create(
        nameof(DragStartedCommand),
        typeof(ICommand),
        typeof(SliderWithTicks));

    public ICommand? DragStartedCommand
    {
        get => (ICommand?)GetValue(DragStartedCommandProperty);
        set => SetValue(DragStartedCommandProperty, value);
    }

    public static readonly BindableProperty DragCompletedCommandProperty = BindableProperty.Create(
        nameof(DragCompletedCommand),
        typeof(ICommand),
        typeof(SliderWithTicks));

    public ICommand? DragCompletedCommand
    {
        get => (ICommand?)GetValue(DragCompletedCommandProperty);
        set => SetValue(DragCompletedCommandProperty, value);
    }

    public static readonly BindableProperty TickValuesProperty = BindableProperty.Create(
        nameof(TickValues),
        typeof(string),
        typeof(SliderWithTicks),
        string.Empty,
        propertyChanged: OnTickValuesChanged);

    public string TickValues
    {
        get => (string)GetValue(TickValuesProperty);
        set => SetValue(TickValuesProperty, value);
    }

    public static readonly BindableProperty TickColorProperty = BindableProperty.Create(
        nameof(TickColor),
        typeof(Color),
        typeof(SliderWithTicks),
        propertyChanged: OnAppearanceChanged);

    public Color? TickColor
    {
        get => (Color?)GetValue(TickColorProperty);
        set => SetValue(TickColorProperty, value);
    }

    private void OnSliderValueChanged(object? sender, ValueChangedEventArgs<double> e)
    {
        if (_isUpdatingFromSlider)
        {
            return;
        }

        _isUpdatingFromSlider = true;
        Value = e.NewValue;
        _isUpdatingFromSlider = false;
    }

    private void OnSliderDragStarted(object? sender, EventArgs e)
    {
        DragStartedCommand?.Execute(null);
        DragStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnSliderDragCompleted(object? sender, EventArgs e)
    {
        DragCompletedCommand?.Execute(null);
        DragCompleted?.Invoke(this, EventArgs.Empty);
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SliderWithTicks slider)
        {
            return;
        }

        if (!slider._isUpdatingFromSlider && Math.Abs(slider._slider.Value - (double)newValue) > double.Epsilon)
        {
            slider._slider.Value = (double)newValue;
        }

        slider.ValueChanged?.Invoke(slider, new ValueChangedEventArgs((double)oldValue, (double)newValue));
    }

    private static void OnRangeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SliderWithTicks slider)
        {
            return;
        }

        slider.UpdateSliderProperties();
        slider.UpdateTickDrawable();
    }

    private static void OnAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SliderWithTicks slider)
        {
            return;
        }

        slider.UpdateSliderProperties();
        slider.UpdateTickDrawable();
    }

    private static void OnTickValuesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SliderWithTicks slider)
        {
            return;
        }

        slider.UpdateTickDrawable();
    }

    private void UpdateSliderProperties()
    {
        _slider.Minimum = Minimum;
        _slider.Maximum = Maximum;
        _slider.Value = Value;
        _slider.MinimumTrackColor = MinimumTrackColor;
        _slider.MaximumTrackColor = MaximumTrackColor;
        _slider.ThumbColor = ThumbColor;
        _slider.ThumbImageSource = ThumbImageSource;
    }

    private void UpdateTickDrawable()
    {
        _tickDrawable.Minimum = Minimum;
        _tickDrawable.Maximum = Maximum;
        _tickDrawable.TickColor = TickColor ?? MaximumTrackColor;
        _tickDrawable.SetTicks(ParseTickValues());
        _ticksView?.Invalidate();
    }

    private IReadOnlyList<double> ParseTickValues()
    {
        if (string.IsNullOrWhiteSpace(TickValues))
        {
            return [];
        }

        var parts = TickValues.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var values = new List<double>(parts.Length);

        foreach (var part in parts)
        {
            if (!double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                continue;
            }

            values.Add(value);
        }

        return values;
    }

}
