using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MaterialDesignThemes.Maui;

public sealed class ColorPicker : TemplatedView
{
    public const string HueSliderPartName = "PART_HueSlider";
    public const string SaturationBrightnessLayoutPartName = "PART_SaturationBrightnessLayout";
    public const string SaturationBrightnessViewPartName = "PART_SaturationBrightnessView";
    public const string SaturationBrightnessThumbPartName = "PART_SaturationBrightnessThumb";

    private const double HueMinimum = 0d;
    private const double HueMaximum = 360d;
    private const double ThumbSize = 18d;
    private const double ThumbRadius = ThumbSize / 2d;

    private AbsoluteLayout? _saturationBrightnessLayout;
    private GraphicsView? _saturationBrightnessView;
    private Border? _saturationBrightnessThumb;
    private Slider? _hueSlider;
    private readonly SaturationBrightnessDrawable _saturationBrightnessDrawable = new();
    private readonly PointerGestureRecognizer? _pointerGesture;
    private readonly PanGestureRecognizer? _panGesture;

    private bool _inCallback;
    private bool _isUpdatingFromHue;
    private bool _isPointerCaptured;
    private Point _panStartPosition;

    public ColorPicker()
    {
        _pointerGesture = new PointerGestureRecognizer();
        _pointerGesture.PointerPressed += OnSaturationBrightnessPointerPressed;
        _pointerGesture.PointerMoved += OnSaturationBrightnessPointerMoved;
        _pointerGesture.PointerReleased += OnSaturationBrightnessPointerReleased;

        _panGesture = new PanGestureRecognizer();
        _panGesture.PanUpdated += OnSaturationBrightnessPanUpdated;
    }

    public event EventHandler<ColorChangedEventArgs>? ColorChanged;

    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
        nameof(Color),
        typeof(Color),
        typeof(ColorPicker),
        Colors.Black,
        BindingMode.TwoWay,
        propertyChanged: OnColorChanged);

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    internal static readonly BindableProperty HsbProperty = BindableProperty.Create(
        nameof(Hsb),
        typeof(Hsb),
        typeof(ColorPicker),
        default(Hsb),
        BindingMode.TwoWay,
        propertyChanged: OnHsbChanged);

    internal Hsb Hsb
    {
        get => (Hsb)GetValue(HsbProperty);
        set => SetValue(HsbProperty, value);
    }

    public static readonly BindableProperty HueSliderPositionProperty = BindableProperty.Create(
        nameof(HueSliderPosition),
        typeof(ColorPickerDock),
        typeof(ColorPicker),
        ColorPickerDock.Bottom,
        propertyChanged: OnHueSliderPositionChanged);

    public ColorPickerDock HueSliderPosition
    {
        get => (ColorPickerDock)GetValue(HueSliderPositionProperty);
        set => SetValue(HueSliderPositionProperty, value);
    }

    public static readonly BindableProperty HueMinimumTrackColorProperty = BindableProperty.Create(
        nameof(HueMinimumTrackColor),
        typeof(Color),
        typeof(ColorPicker),
        null,
        propertyChanged: OnHueSliderAppearanceChanged);

    public Color? HueMinimumTrackColor
    {
        get => (Color?)GetValue(HueMinimumTrackColorProperty);
        set => SetValue(HueMinimumTrackColorProperty, value);
    }

    public static readonly BindableProperty HueMaximumTrackColorProperty = BindableProperty.Create(
        nameof(HueMaximumTrackColor),
        typeof(Color),
        typeof(ColorPicker),
        null,
        propertyChanged: OnHueSliderAppearanceChanged);

    public Color? HueMaximumTrackColor
    {
        get => (Color?)GetValue(HueMaximumTrackColorProperty);
        set => SetValue(HueMaximumTrackColorProperty, value);
    }

    public static readonly BindableProperty HueThumbColorProperty = BindableProperty.Create(
        nameof(HueThumbColor),
        typeof(Color),
        typeof(ColorPicker),
        null,
        propertyChanged: OnHueSliderAppearanceChanged);

    public Color? HueThumbColor
    {
        get => (Color?)GetValue(HueThumbColorProperty);
        set => SetValue(HueThumbColorProperty, value);
    }

    private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ColorPicker colorPicker)
        {
            return;
        }

        if (colorPicker._inCallback)
        {
            return;
        }

        colorPicker._inCallback = true;
        colorPicker.Hsb = ((Color)newValue).ToHsb();
        colorPicker.ColorChanged?.Invoke(colorPicker, new ColorChangedEventArgs((Color)oldValue, (Color)newValue));
        colorPicker._inCallback = false;
    }

    private static void OnHsbChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ColorPicker colorPicker)
        {
            return;
        }

        if (!colorPicker._inCallback)
        {
            colorPicker._inCallback = true;
            colorPicker.Color = ((Hsb)newValue).ToColor();
            colorPicker._inCallback = false;
        }

        colorPicker.UpdateFromHsb();
    }

    private static void OnHueSliderPositionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ColorPicker colorPicker)
        {
            return;
        }

        colorPicker.UpdateLayoutForHuePosition();
    }

    private static void OnHueSliderAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ColorPicker colorPicker)
        {
            colorPicker.UpdateHueSliderAppearance();
        }
    }

    private void OnHueSliderValueChanged(object? sender, ValueChangedEventArgs<double> e)
    {
        if (_isUpdatingFromHue)
        {
            return;
        }

        Hsb = new Hsb(e.NewValue, Hsb.Saturation, Hsb.Brightness);
    }

    private void OnSaturationBrightnessPointerPressed(object? sender, PointerEventArgs e)
    {
        var position = e.GetPosition(_saturationBrightnessView);
        if (position is null)
        {
            return;
        }

        _isPointerCaptured = true;
        ApplyThumbPosition(position.Value.X, position.Value.Y);
    }

    private void OnSaturationBrightnessPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isPointerCaptured)
        {
            return;
        }

        var position = e.GetPosition(_saturationBrightnessView);
        if (position is null)
        {
            return;
        }

        ApplyThumbPosition(position.Value.X, position.Value.Y);
    }

    private void OnSaturationBrightnessPointerReleased(object? sender, PointerEventArgs e)
    {
        _isPointerCaptured = false;
    }

    private void OnSaturationBrightnessPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_saturationBrightnessLayout is null || _saturationBrightnessLayout.Width <= 0 || _saturationBrightnessLayout.Height <= 0)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _panStartPosition = GetThumbCenter();
                ApplyThumbPosition(_panStartPosition.X, _panStartPosition.Y);
                break;
            case GestureStatus.Running:
                ApplyThumbPosition(_panStartPosition.X + e.TotalX, _panStartPosition.Y + e.TotalY);
                break;
        }
    }

    private void OnSaturationBrightnessLayoutSizeChanged(object? sender, EventArgs e)
    {
        SetThumbPosition();
    }

    private void ApplyThumbPosition(double left, double top)
    {
        if (_saturationBrightnessLayout is null || _saturationBrightnessLayout.Width <= 0 || _saturationBrightnessLayout.Height <= 0)
        {
            return;
        }

        var clampedLeft = Math.Clamp(left, 0, _saturationBrightnessLayout.Width);
        var clampedTop = Math.Clamp(top, 0, _saturationBrightnessLayout.Height);

        SetThumbPosition(clampedLeft, clampedTop);

        var saturation = clampedLeft / _saturationBrightnessLayout.Width;
        var brightness = 1 - (clampedTop / _saturationBrightnessLayout.Height);

        if (Math.Abs(Hsb.Saturation - saturation) > double.Epsilon || Math.Abs(Hsb.Brightness - brightness) > double.Epsilon)
        {
            Hsb = new Hsb(Hsb.Hue, saturation, brightness);
        }
    }

    private void SetThumbPosition()
    {
        if (_saturationBrightnessLayout is null || _saturationBrightnessLayout.Width <= 0 || _saturationBrightnessLayout.Height <= 0)
        {
            return;
        }

        var left = _saturationBrightnessLayout.Width * Hsb.Saturation;
        var top = _saturationBrightnessLayout.Height * (1 - Hsb.Brightness);
        SetThumbPosition(left, top);
    }

    private Point GetThumbCenter()
    {
        if (_saturationBrightnessLayout is null)
        {
            return new Point(0, 0);
        }

        var left = _saturationBrightnessLayout.Width * Hsb.Saturation;
        var top = _saturationBrightnessLayout.Height * (1 - Hsb.Brightness);
        return new Point(left, top);
    }

    private void SetThumbPosition(double left, double top)
    {
        if (_saturationBrightnessLayout is null || _saturationBrightnessThumb is null)
        {
            return;
        }

        var clampedLeft = Math.Clamp(left, 0, _saturationBrightnessLayout.Width);
        var clampedTop = Math.Clamp(top, 0, _saturationBrightnessLayout.Height);

        var thumbLeft = clampedLeft - ThumbRadius;
        var thumbTop = clampedTop - ThumbRadius;

        AbsoluteLayout.SetLayoutBounds(_saturationBrightnessThumb, new Rect(thumbLeft, thumbTop, ThumbSize, ThumbSize));
    }

    private void UpdateFromHsb()
    {
        UpdateHueSliderAppearance();
        UpdateHueSliderValue();
        UpdateSaturationBrightnessDrawable();
        SetThumbPosition();
    }

    private void UpdateHueSliderValue()
    {
        if (_hueSlider is null || Math.Abs(_hueSlider.Value - Hsb.Hue) < double.Epsilon)
        {
            return;
        }

        _isUpdatingFromHue = true;
        _hueSlider.Value = Hsb.Hue;
        _isUpdatingFromHue = false;
    }

    private void UpdateHueSliderAppearance()
    {
        if (_hueSlider is null)
        {
            return;
        }

        _hueSlider.MinimumTrackColor = HueMinimumTrackColor;
        _hueSlider.MaximumTrackColor = HueMaximumTrackColor;
        _hueSlider.ThumbColor = HueThumbColor;
    }

    private void UpdateSaturationBrightnessDrawable()
    {
        _saturationBrightnessDrawable.HueColor = new Hsb(Hsb.Hue, 1, 1).ToColor();
        _saturationBrightnessView?.Invalidate();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_saturationBrightnessLayout is not null)
        {
            _saturationBrightnessLayout.SizeChanged -= OnSaturationBrightnessLayoutSizeChanged;
        }

        if (_saturationBrightnessView is not null)
        {
            if (_pointerGesture is not null)
            {
                _saturationBrightnessView.GestureRecognizers.Remove(_pointerGesture);
            }
            if (_panGesture is not null)
            {
                _saturationBrightnessView.GestureRecognizers.Remove(_panGesture);
            }
        }

        if (_hueSlider is not null)
        {
            _hueSlider.ValueChanged -= OnHueSliderValueChanged;
        }

        _saturationBrightnessLayout = GetTemplateChild(SaturationBrightnessLayoutPartName) as AbsoluteLayout;
        _saturationBrightnessView = GetTemplateChild(SaturationBrightnessViewPartName) as GraphicsView;
        _saturationBrightnessThumb = GetTemplateChild(SaturationBrightnessThumbPartName) as Border;
        _hueSlider = GetTemplateChild(HueSliderPartName) as Slider;

        if (_saturationBrightnessView is not null)
        {
            _saturationBrightnessView.Drawable = _saturationBrightnessDrawable;
            if (_pointerGesture is not null)
            {
                _saturationBrightnessView.GestureRecognizers.Add(_pointerGesture);
            }
            if (_panGesture is not null)
            {
                _saturationBrightnessView.GestureRecognizers.Add(_panGesture);
            }
        }

        if (_saturationBrightnessThumb is not null)
        {
            _saturationBrightnessThumb.HeightRequest = ThumbSize;
            _saturationBrightnessThumb.WidthRequest = ThumbSize;
            _saturationBrightnessThumb.Stroke = Colors.White;
            _saturationBrightnessThumb.StrokeThickness = 2;
            _saturationBrightnessThumb.StrokeShape = new RoundRectangle
            {
                CornerRadius = ThumbRadius
            };
            _saturationBrightnessThumb.BackgroundColor = Colors.Transparent;
            AbsoluteLayout.SetLayoutFlags(_saturationBrightnessThumb, AbsoluteLayoutFlags.None);
        }

        if (_saturationBrightnessLayout is not null)
        {
            _saturationBrightnessLayout.SizeChanged += OnSaturationBrightnessLayoutSizeChanged;
        }

        if (_hueSlider is not null)
        {
            _hueSlider.Minimum = HueMinimum;
            _hueSlider.Maximum = HueMaximum;
            _hueSlider.ValueChanged += OnHueSliderValueChanged;
        }

        UpdateLayoutForHuePosition();
        UpdateFromHsb();
    }

    private void UpdateLayoutForHuePosition()
    {
        if (_hueSlider is null || _saturationBrightnessLayout is null)
        {
            return;
        }

        switch (HueSliderPosition)
        {
            case ColorPickerDock.Top:
                Grid.SetRow(_hueSlider, 0);
                Grid.SetColumn(_hueSlider, 0);
                Grid.SetColumnSpan(_hueSlider, 2);
                Grid.SetRowSpan(_hueSlider, 1);
                Grid.SetRow(_saturationBrightnessLayout, 1);
                Grid.SetColumn(_saturationBrightnessLayout, 0);
                Grid.SetColumnSpan(_saturationBrightnessLayout, 2);
                Grid.SetRowSpan(_saturationBrightnessLayout, 1);
                break;
            case ColorPickerDock.Left:
                Grid.SetRow(_hueSlider, 0);
                Grid.SetColumn(_hueSlider, 0);
                Grid.SetColumnSpan(_hueSlider, 1);
                Grid.SetRowSpan(_hueSlider, 2);
                Grid.SetRow(_saturationBrightnessLayout, 0);
                Grid.SetColumn(_saturationBrightnessLayout, 1);
                Grid.SetColumnSpan(_saturationBrightnessLayout, 1);
                Grid.SetRowSpan(_saturationBrightnessLayout, 2);
                break;
            case ColorPickerDock.Right:
                Grid.SetRow(_hueSlider, 0);
                Grid.SetColumn(_hueSlider, 1);
                Grid.SetColumnSpan(_hueSlider, 1);
                Grid.SetRowSpan(_hueSlider, 2);
                Grid.SetRow(_saturationBrightnessLayout, 0);
                Grid.SetColumn(_saturationBrightnessLayout, 0);
                Grid.SetColumnSpan(_saturationBrightnessLayout, 1);
                Grid.SetRowSpan(_saturationBrightnessLayout, 2);
                break;
            default:
                Grid.SetRow(_hueSlider, 1);
                Grid.SetColumn(_hueSlider, 0);
                Grid.SetColumnSpan(_hueSlider, 2);
                Grid.SetRowSpan(_hueSlider, 1);
                Grid.SetRow(_saturationBrightnessLayout, 0);
                Grid.SetColumn(_saturationBrightnessLayout, 0);
                Grid.SetColumnSpan(_saturationBrightnessLayout, 2);
                Grid.SetRowSpan(_saturationBrightnessLayout, 1);
                break;
        }
    }
}

public enum ColorPickerDock
{
    Left,
    Top,
    Right,
    Bottom
}

public sealed class ColorChangedEventArgs(Color oldColor, Color newColor) : EventArgs
{
    public Color OldColor { get; } = oldColor;
    public Color NewColor { get; } = newColor;
}

public readonly record struct Hsb(double Hue, double Saturation, double Brightness);

public static class HsbExtensions
{
    public static Color ToColor(this Hsb hsv)
    {
        var h = hsv.Hue;
        var s = hsv.Saturation;
        var b = hsv.Brightness;

        if (s.IsCloseTo(0))
        {
            return Color.FromRgb(ToByte(b), ToByte(b), ToByte(b));
        }

        if (h.IsCloseTo(360))
        {
            h = 0;
        }

        while (h > 360)
        {
            h -= 360;
        }

        while (h < 0)
        {
            h += 360;
        }

        h /= 60;

        var i = (int)Math.Floor(h);
        var f = h - i;
        var p = b * (1 - s);
        var q = b * (1 - s * f);
        var t = b * (1 - s * (1 - f));

        return i switch
        {
            0 => Color.FromRgb(ToByte(b), ToByte(t), ToByte(p)),
            1 => Color.FromRgb(ToByte(q), ToByte(b), ToByte(p)),
            2 => Color.FromRgb(ToByte(p), ToByte(b), ToByte(t)),
            3 => Color.FromRgb(ToByte(p), ToByte(q), ToByte(b)),
            4 => Color.FromRgb(ToByte(t), ToByte(p), ToByte(b)),
            5 => Color.FromRgb(ToByte(b), ToByte(p), ToByte(q)),
            _ => throw new InvalidOperationException("Invalid HSB values")
        };
    }

    public static Hsb ToHsb(this Color color)
    {
        var r = color.Red;
        var g = color.Green;
        var b = color.Blue;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var v = max;
        var h = max;

        var d = max - min;
        var s = max.IsCloseTo(0) ? 0 : d / max;

        if (max.IsCloseTo(min))
        {
            h = 0;
        }
        else
        {
            if (max.IsCloseTo(r))
            {
                h = (g - b) / d + (g < b ? 6 : 0);
            }
            else if (max.IsCloseTo(g))
            {
                h = (b - r) / d + 2;
            }
            else if (max.IsCloseTo(b))
            {
                h = (r - g) / d + 4;
            }

            h *= 60;
        }

        return new Hsb(h, s, v);
    }

    private static byte ToByte(double value)
    {
        var clamped = Math.Clamp((int)Math.Round(value * 255), 0, 255);
        return (byte)clamped;
    }

    private static bool IsCloseTo(this double value, double target, double tolerance = double.Epsilon)
        => Math.Abs(value - target) < tolerance;

    private static bool IsCloseTo(this float value, float target, float tolerance = float.Epsilon)
        => Math.Abs(value - target) < tolerance;
}

public sealed class SaturationBrightnessDrawable : IDrawable
{
    public Color HueColor { get; set; } = Colors.Red;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var saturationGradient = new LinearGradientPaint
        {
            StartColor = Colors.White,
            EndColor = HueColor,
            StartPoint = new PointF(dirtyRect.Left, dirtyRect.Top),
            EndPoint = new PointF(dirtyRect.Right, dirtyRect.Top)
        };

        canvas.SetFillPaint(saturationGradient, dirtyRect);
        canvas.FillRectangle(dirtyRect);

        var brightnessGradient = new LinearGradientPaint
        {
            StartColor = Colors.Transparent,
            EndColor = Colors.Black,
            StartPoint = new PointF(dirtyRect.Left, dirtyRect.Top),
            EndPoint = new PointF(dirtyRect.Left, dirtyRect.Bottom)
        };

        canvas.SetFillPaint(brightnessGradient, dirtyRect);
        canvas.FillRectangle(dirtyRect);
    }
}
