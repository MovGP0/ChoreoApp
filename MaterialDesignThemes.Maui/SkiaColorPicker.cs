using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SKPaintSurfaceEventArgs = SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs;

namespace MaterialDesignThemes.Maui;

public sealed class SkiaColorPicker : ContentView
{
    private const double HueMinimum = 0d;
    private const double HueMaximum = 360d;
    private const double DefaultWheelMinimumSize = 160d;
    private const double DefaultSelectionThumbSize = 18d;
    private const double DefaultSelectionStrokeThickness = 2d;

    private static readonly SKColor[] s_hueColors =
    [
        new SKColor(255, 0, 0),
        new SKColor(255, 255, 0),
        new SKColor(0, 255, 0),
        new SKColor(0, 255, 255),
        new SKColor(0, 0, 255),
        new SKColor(255, 0, 255),
        new SKColor(255, 0, 0)
    ];

    private readonly Grid _layoutGrid;
    private readonly SKCanvasView _wheelView;
    private readonly Slider _valueSlider;

    private Hsb _hsb;
    private bool _inCallback;
    private bool _isUpdatingFromSlider;
    private bool _isInitialized;
    private long? _activeTouchId;

    public SkiaColorPicker()
    {
        MaximumWidthRequest = 240d;

        _wheelView = new SKCanvasView
        {
            EnableTouchEvents = true
        };
        _wheelView.PaintSurface += OnWheelPaintSurface;
        _wheelView.Touch += OnWheelTouch;

        _valueSlider = new Slider
        {
            Minimum = 0,
            Maximum = 1
        };
        _valueSlider.ValueChanged += OnValueSliderValueChanged;

        _layoutGrid = new Grid
        {
            RowDefinitions =
            [
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star }
            ],
            ColumnDefinitions =
            [
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            ]
        };

        _layoutGrid.Children.Add(_wheelView);
        _layoutGrid.Children.Add(_valueSlider);
        Content = _layoutGrid;

        _hsb = Color.ToHsb();
        _isInitialized = true;
        UpdateWheelMinimumSize();
        UpdateSliderAppearance();
        UpdateSliderValue();
        UpdateLayoutForSliderPosition();
    }

    public event EventHandler<ColorChangedEventArgs>? ColorChanged;

    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
        nameof(Color),
        typeof(Color),
        typeof(SkiaColorPicker),
        Colors.Black,
        BindingMode.TwoWay,
        propertyChanged: OnColorChanged);

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly BindableProperty WheelMinimumWidthProperty = BindableProperty.Create(
        nameof(WheelMinimumWidth),
        typeof(double),
        typeof(SkiaColorPicker),
        DefaultWheelMinimumSize,
        propertyChanged: OnWheelMinimumSizeChanged);

    public double WheelMinimumWidth
    {
        get => (double)GetValue(WheelMinimumWidthProperty);
        set => SetValue(WheelMinimumWidthProperty, value);
    }

    public static readonly BindableProperty WheelMinimumHeightProperty = BindableProperty.Create(
        nameof(WheelMinimumHeight),
        typeof(double),
        typeof(SkiaColorPicker),
        DefaultWheelMinimumSize,
        propertyChanged: OnWheelMinimumSizeChanged);

    public double WheelMinimumHeight
    {
        get => (double)GetValue(WheelMinimumHeightProperty);
        set => SetValue(WheelMinimumHeightProperty, value);
    }

    public static readonly BindableProperty ValueSliderPositionProperty = BindableProperty.Create(
        nameof(ValueSliderPosition),
        typeof(ColorPickerDock),
        typeof(SkiaColorPicker),
        ColorPickerDock.Bottom,
        propertyChanged: OnValueSliderPositionChanged);

    public ColorPickerDock ValueSliderPosition
    {
        get => (ColorPickerDock)GetValue(ValueSliderPositionProperty);
        set => SetValue(ValueSliderPositionProperty, value);
    }

    public static readonly BindableProperty SliderMinimumTrackColorProperty = BindableProperty.Create(
        nameof(SliderMinimumTrackColor),
        typeof(Color),
        typeof(SkiaColorPicker),
        null,
        propertyChanged: OnSliderAppearanceChanged);

    public Color? SliderMinimumTrackColor
    {
        get => (Color?)GetValue(SliderMinimumTrackColorProperty);
        set => SetValue(SliderMinimumTrackColorProperty, value);
    }

    public static readonly BindableProperty SliderMaximumTrackColorProperty = BindableProperty.Create(
        nameof(SliderMaximumTrackColor),
        typeof(Color),
        typeof(SkiaColorPicker),
        null,
        propertyChanged: OnSliderAppearanceChanged);

    public Color? SliderMaximumTrackColor
    {
        get => (Color?)GetValue(SliderMaximumTrackColorProperty);
        set => SetValue(SliderMaximumTrackColorProperty, value);
    }

    public static readonly BindableProperty SliderThumbColorProperty = BindableProperty.Create(
        nameof(SliderThumbColor),
        typeof(Color),
        typeof(SkiaColorPicker),
        null,
        propertyChanged: OnSliderAppearanceChanged);

    public Color? SliderThumbColor
    {
        get => (Color?)GetValue(SliderThumbColorProperty);
        set => SetValue(SliderThumbColorProperty, value);
    }

    public static readonly BindableProperty SelectionThumbSizeProperty = BindableProperty.Create(
        nameof(SelectionThumbSize),
        typeof(double),
        typeof(SkiaColorPicker),
        DefaultSelectionThumbSize,
        propertyChanged: OnSelectionAppearanceChanged);

    public double SelectionThumbSize
    {
        get => (double)GetValue(SelectionThumbSizeProperty);
        set => SetValue(SelectionThumbSizeProperty, value);
    }

    public static readonly BindableProperty SelectionStrokeColorProperty = BindableProperty.Create(
        nameof(SelectionStrokeColor),
        typeof(Color),
        typeof(SkiaColorPicker),
        Colors.White,
        propertyChanged: OnSelectionAppearanceChanged);

    public Color SelectionStrokeColor
    {
        get => (Color)GetValue(SelectionStrokeColorProperty);
        set => SetValue(SelectionStrokeColorProperty, value);
    }

    public static readonly BindableProperty SelectionStrokeThicknessProperty = BindableProperty.Create(
        nameof(SelectionStrokeThickness),
        typeof(double),
        typeof(SkiaColorPicker),
        DefaultSelectionStrokeThickness,
        propertyChanged: OnSelectionAppearanceChanged);

    public double SelectionStrokeThickness
    {
        get => (double)GetValue(SelectionStrokeThicknessProperty);
        set => SetValue(SelectionStrokeThicknessProperty, value);
    }

    private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SkiaColorPicker colorPicker)
        {
            return;
        }

        if (colorPicker._inCallback)
        {
            return;
        }

        colorPicker._inCallback = true;
        colorPicker._hsb = ((Color)newValue).ToHsb();
        colorPicker.UpdateSliderValue();
        colorPicker._wheelView.InvalidateSurface();
        colorPicker._inCallback = false;

        colorPicker.ColorChanged?.Invoke(
            colorPicker,
            new ColorChangedEventArgs((Color)oldValue, (Color)newValue));
    }

    private static void OnWheelMinimumSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SkiaColorPicker colorPicker)
        {
            colorPicker.UpdateWheelMinimumSize();
        }
    }

    private static void OnValueSliderPositionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SkiaColorPicker colorPicker)
        {
            colorPicker.UpdateLayoutForSliderPosition();
        }
    }

    private static void OnSliderAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SkiaColorPicker colorPicker)
        {
            colorPicker.UpdateSliderAppearance();
        }
    }

    private static void OnSelectionAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SkiaColorPicker colorPicker)
        {
            if (!colorPicker._isInitialized)
            {
                return;
            }

            colorPicker._wheelView.InvalidateSurface();
        }
    }

    private void OnValueSliderValueChanged(object? sender, ValueChangedEventArgs<double> e)
    {
        if (_isUpdatingFromSlider)
        {
            return;
        }

        var newHsb = new Hsb(_hsb.Hue, _hsb.Saturation, e.NewValue);
        UpdateColorFromHsb(newHsb);
    }

    private void OnWheelTouch(object? sender, SKTouchEventArgs e)
    {
        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
                _activeTouchId = e.Id;
                TryUpdateFromTouch(e.Location);
                e.Handled = true;
                return;
            case SKTouchAction.Moved:
                if (_activeTouchId == e.Id)
                {
                    TryUpdateFromTouch(e.Location);
                    e.Handled = true;
                    return;
                }
                break;
            case SKTouchAction.Released:
            case SKTouchAction.Cancelled:
                if (_activeTouchId == e.Id)
                {
                    _activeTouchId = null;
                    e.Handled = true;
                    return;
                }
                break;
        }
    }

    private void TryUpdateFromTouch(SKPoint location)
    {
        if (TryGetHueSaturation(location, out var hue, out var saturation))
        {
            var newHsb = new Hsb(hue, saturation, _hsb.Brightness);
            UpdateColorFromHsb(newHsb);
        }
    }

    private void UpdateColorFromHsb(Hsb newHsb)
    {
        var oldColor = Color;
        if (newHsb == _hsb)
        {
            return;
        }

        _hsb = newHsb;
        var newColor = newHsb.ToColor();

        _inCallback = true;
        SetValue(ColorProperty, newColor);
        _inCallback = false;

        UpdateSliderValue();
        _wheelView.InvalidateSurface();

        if (oldColor != newColor)
        {
            ColorChanged?.Invoke(this, new ColorChangedEventArgs(oldColor, newColor));
        }
    }

    private void UpdateWheelMinimumSize()
    {
        if (!_isInitialized)
        {
            return;
        }

        _wheelView.MinimumWidthRequest = WheelMinimumWidth;
        _wheelView.MinimumHeightRequest = WheelMinimumHeight;
    }

    private void UpdateSliderValue()
    {
        if (!_isInitialized)
        {
            return;
        }

        if (Math.Abs(_valueSlider.Value - _hsb.Brightness) < double.Epsilon)
        {
            return;
        }

        _isUpdatingFromSlider = true;
        _valueSlider.Value = _hsb.Brightness;
        _isUpdatingFromSlider = false;
    }

    private void UpdateSliderAppearance()
    {
        if (!_isInitialized)
        {
            return;
        }

        _valueSlider.MinimumTrackColor = SliderMinimumTrackColor;
        _valueSlider.MaximumTrackColor = SliderMaximumTrackColor;
        _valueSlider.ThumbColor = SliderThumbColor;
    }

    private void UpdateLayoutForSliderPosition()
    {
        if (!_isInitialized)
        {
            return;
        }

        _valueSlider.Rotation = 0;
        _valueSlider.HorizontalOptions = LayoutOptions.Fill;
        _valueSlider.VerticalOptions = LayoutOptions.Center;

        switch (ValueSliderPosition)
        {
            case ColorPickerDock.Top:
                Grid.SetRow(_valueSlider, 0);
                Grid.SetColumn(_valueSlider, 0);
                Grid.SetColumnSpan(_valueSlider, 2);
                Grid.SetRowSpan(_valueSlider, 1);
                Grid.SetRow(_wheelView, 1);
                Grid.SetColumn(_wheelView, 0);
                Grid.SetColumnSpan(_wheelView, 2);
                Grid.SetRowSpan(_wheelView, 1);
                break;
            case ColorPickerDock.Left:
                _valueSlider.Rotation = -90;
                _valueSlider.HorizontalOptions = LayoutOptions.Center;
                _valueSlider.VerticalOptions = LayoutOptions.Fill;
                Grid.SetRow(_valueSlider, 0);
                Grid.SetColumn(_valueSlider, 0);
                Grid.SetColumnSpan(_valueSlider, 1);
                Grid.SetRowSpan(_valueSlider, 2);
                Grid.SetRow(_wheelView, 0);
                Grid.SetColumn(_wheelView, 1);
                Grid.SetColumnSpan(_wheelView, 1);
                Grid.SetRowSpan(_wheelView, 2);
                break;
            case ColorPickerDock.Right:
                _valueSlider.Rotation = 90;
                _valueSlider.HorizontalOptions = LayoutOptions.Center;
                _valueSlider.VerticalOptions = LayoutOptions.Fill;
                Grid.SetRow(_valueSlider, 0);
                Grid.SetColumn(_valueSlider, 1);
                Grid.SetColumnSpan(_valueSlider, 1);
                Grid.SetRowSpan(_valueSlider, 2);
                Grid.SetRow(_wheelView, 0);
                Grid.SetColumn(_wheelView, 0);
                Grid.SetColumnSpan(_wheelView, 1);
                Grid.SetRowSpan(_wheelView, 2);
                break;
            default:
                Grid.SetRow(_valueSlider, 1);
                Grid.SetColumn(_valueSlider, 0);
                Grid.SetColumnSpan(_valueSlider, 2);
                Grid.SetRowSpan(_valueSlider, 1);
                Grid.SetRow(_wheelView, 0);
                Grid.SetColumn(_wheelView, 0);
                Grid.SetColumnSpan(_wheelView, 2);
                Grid.SetRowSpan(_wheelView, 1);
                break;
        }
    }

    private void OnWheelPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        var info = e.Info;
        var diameter = Math.Min(info.Width, info.Height);
        if (diameter <= 0)
        {
            return;
        }

        var center = new SKPoint(info.Width / 2f, info.Height / 2f);
        var radius = diameter / 2f;

        using var huePaint = new SKPaint();
        huePaint.IsAntialias = true;
        huePaint.Style = SKPaintStyle.Fill;
        huePaint.Shader = SKShader.CreateSweepGradient(center, s_hueColors);
        canvas.DrawCircle(center, radius, huePaint);

        using var saturationPaint = new SKPaint();
        saturationPaint.IsAntialias = true;
        saturationPaint.Style = SKPaintStyle.Fill;
        saturationPaint.Shader = SKShader.CreateRadialGradient(
            center,
            radius,
            [SKColors.White, SKColors.Transparent],
            [0f, 1f],
            SKShaderTileMode.Clamp);
        canvas.DrawCircle(center, radius, saturationPaint);

        DrawSelectionThumb(canvas, center, radius, info);
    }

    private void DrawSelectionThumb(SKCanvas canvas, SKPoint center, float radius, SKImageInfo info)
    {
        if (_wheelView.Width <= 0 || _wheelView.Height <= 0)
        {
            return;
        }

        var angleRadians = _hsb.Hue * Math.PI / 180d;
        var distance = radius * (float)Math.Clamp(_hsb.Saturation, 0, 1);
        var x = center.X + (float)(Math.Cos(angleRadians) * distance);
        var y = center.Y + (float)(Math.Sin(angleRadians) * distance);

        var scale = GetCanvasScale(info);
        var thumbRadius = (float)(SelectionThumbSize / 2d) * scale;
        var strokeThickness = (float)SelectionStrokeThickness * scale;

        using var fillPaint = new SKPaint();
        fillPaint.IsAntialias = true;
        fillPaint.Style = SKPaintStyle.Fill;
        fillPaint.Color = ToSkColor(_hsb.ToColor());
        canvas.DrawCircle(x, y, thumbRadius, fillPaint);

        using var strokePaint = new SKPaint();
        strokePaint.IsAntialias = true;
        strokePaint.Style = SKPaintStyle.Stroke;
        strokePaint.StrokeWidth = strokeThickness;
        strokePaint.Color = ToSkColor(SelectionStrokeColor);
        canvas.DrawCircle(x, y, thumbRadius, strokePaint);
    }

    private bool TryGetHueSaturation(SKPoint location, out double hue, out double saturation)
    {
        var size = _wheelView.CanvasSize;
        var diameter = Math.Min(size.Width, size.Height);
        if (diameter <= 0)
        {
            hue = HueMinimum;
            saturation = 0;
            return false;
        }

        var center = new SKPoint(size.Width / 2f, size.Height / 2f);
        var dx = location.X - center.X;
        var dy = location.Y - center.Y;
        var distance = MathF.Sqrt((dx * dx) + (dy * dy));
        var radius = diameter / 2f;

        var angle = Math.Atan2(dy, dx) * 180d / Math.PI;
        if (angle < 0)
        {
            angle += HueMaximum;
        }

        hue = angle;
        saturation = Math.Clamp(distance / radius, 0, 1);
        return true;
    }

    private float GetCanvasScale(SKImageInfo info)
    {
        var widthScale = (float)(info.Width / Math.Max(1d, _wheelView.Width));
        var heightScale = (float)(info.Height / Math.Max(1d, _wheelView.Height));
        return Math.Min(widthScale, heightScale);
    }

    private static SKColor ToSkColor(Color color)
    {
        var r = Math.Clamp((int)Math.Round(color.Red * 255), 0, 255);
        var g = Math.Clamp((int)Math.Round(color.Green * 255), 0, 255);
        var b = Math.Clamp((int)Math.Round(color.Blue * 255), 0, 255);
        var a = Math.Clamp((int)Math.Round(color.Alpha * 255), 0, 255);
        return new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
    }
}
