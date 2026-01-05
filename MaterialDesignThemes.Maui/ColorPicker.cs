using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MaterialDesignThemes.Maui;

public sealed class ColorPicker : ContentView
{
    private const double HueMinimum = 0d;
    private const double HueMaximum = 360d;
    private const double ThumbSize = 18d;
    private const double ThumbRadius = ThumbSize / 2d;

    private readonly Grid _layout = [];
    private readonly AbsoluteLayout _saturationBrightnessLayout = [];
    private readonly GraphicsView _saturationBrightnessView;
    private readonly Border _saturationBrightnessThumb;
    private readonly Slider _hueSlider = new();
    private readonly SaturationBrightnessDrawable _saturationBrightnessDrawable = new();

    private bool _inCallback;
    private bool _isUpdatingFromHue;
    private bool _isPointerCaptured;
    private Point _panStartPosition;

    public ColorPicker()
    {
        _saturationBrightnessView = new GraphicsView
        {
            Drawable = _saturationBrightnessDrawable
        };

        _saturationBrightnessThumb = new Border
        {
            HeightRequest = ThumbSize,
            WidthRequest = ThumbSize,
            Stroke = Colors.White,
            StrokeThickness = 2,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = ThumbRadius
            },
            BackgroundColor = Colors.Transparent
        };

        AbsoluteLayout.SetLayoutFlags(_saturationBrightnessView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(_saturationBrightnessView, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(_saturationBrightnessThumb, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(_saturationBrightnessThumb, new Rect(0, 0, ThumbSize, ThumbSize));

        var pointerGesture = new PointerGestureRecognizer();
        pointerGesture.PointerPressed += OnSaturationBrightnessPointerPressed;
        pointerGesture.PointerMoved += OnSaturationBrightnessPointerMoved;
        pointerGesture.PointerReleased += OnSaturationBrightnessPointerReleased;
        _saturationBrightnessView.GestureRecognizers.Add(pointerGesture);

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnSaturationBrightnessPanUpdated;
        _saturationBrightnessView.GestureRecognizers.Add(panGesture);

        _saturationBrightnessLayout.Children.Add(_saturationBrightnessView);
        _saturationBrightnessLayout.Children.Add(_saturationBrightnessThumb);
        _saturationBrightnessLayout.SizeChanged += OnSaturationBrightnessLayoutSizeChanged;

        _hueSlider.Minimum = HueMinimum;
        _hueSlider.Maximum = HueMaximum;
        _hueSlider.ValueChanged += OnHueSliderValueChanged;

        Content = _layout;
        BuildLayout();
        UpdateFromHsb();
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

        colorPicker.BuildLayout();
    }

    private void BuildLayout()
    {
        _layout.RowDefinitions.Clear();
        _layout.ColumnDefinitions.Clear();
        _layout.Children.Clear();

        switch (HueSliderPosition)
        {
            case ColorPickerDock.Top:
                _layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                _layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                _layout.Add(_hueSlider, 0);
                _layout.Add(_saturationBrightnessLayout, 0, 1);
                break;
            case ColorPickerDock.Left:
                _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                _layout.Add(_hueSlider, 0);
                _layout.Add(_saturationBrightnessLayout, 1);
                break;
            case ColorPickerDock.Right:
                _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                _layout.Add(_saturationBrightnessLayout, 0);
                _layout.Add(_hueSlider, 1);
                break;
            default:
                _layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                _layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                _layout.Add(_saturationBrightnessLayout, 0);
                _layout.Add(_hueSlider, 0, 1);
                break;
        }
    }

    private void OnHueSliderValueChanged(object? sender, ValueChangedEventArgs e)
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
        if (_saturationBrightnessLayout.Width <= 0 || _saturationBrightnessLayout.Height <= 0)
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
        if (_saturationBrightnessLayout.Width <= 0 || _saturationBrightnessLayout.Height <= 0)
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
        if (_saturationBrightnessLayout.Width <= 0 || _saturationBrightnessLayout.Height <= 0)
        {
            return;
        }

        var left = _saturationBrightnessLayout.Width * Hsb.Saturation;
        var top = _saturationBrightnessLayout.Height * (1 - Hsb.Brightness);
        SetThumbPosition(left, top);
    }

    private Point GetThumbCenter()
    {
        var left = _saturationBrightnessLayout.Width * Hsb.Saturation;
        var top = _saturationBrightnessLayout.Height * (1 - Hsb.Brightness);
        return new Point(left, top);
    }

    private void SetThumbPosition(double left, double top)
    {
        var clampedLeft = Math.Clamp(left, 0, _saturationBrightnessLayout.Width);
        var clampedTop = Math.Clamp(top, 0, _saturationBrightnessLayout.Height);

        var thumbLeft = clampedLeft - ThumbRadius;
        var thumbTop = clampedTop - ThumbRadius;

        AbsoluteLayout.SetLayoutBounds(_saturationBrightnessThumb, new Rect(thumbLeft, thumbTop, ThumbSize, ThumbSize));
    }

    private void UpdateFromHsb()
    {
        UpdateHueSliderValue();
        UpdateSaturationBrightnessDrawable();
        SetThumbPosition();
    }

    private void UpdateHueSliderValue()
    {
        if (Math.Abs(_hueSlider.Value - Hsb.Hue) < double.Epsilon)
        {
            return;
        }

        _isUpdatingFromHue = true;
        _hueSlider.Value = Hsb.Hue;
        _isUpdatingFromHue = false;
    }

    private void UpdateSaturationBrightnessDrawable()
    {
        _saturationBrightnessDrawable.HueColor = new Hsb(Hsb.Hue, 1, 1).ToColor();
        _saturationBrightnessView.Invalidate();
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
