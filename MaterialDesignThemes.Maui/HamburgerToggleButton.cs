using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Minimal toggle button for MAUI (Border-based) with hamburger-to-arrow animation akin to MaterialDesign.
/// Exposes IsChecked, Checked/Unchecked, Command, and palette properties.
/// </summary>
public sealed class HamburgerToggleButton : Border
{
    private const uint DefaultAnimationDuration = 1600;
    private const double MinBarThickness = 1;
    private const double MinBarInset = 1;

    private readonly TapGestureRecognizer _tap = new();
    private readonly Line _bar1 = CreateBar();
    private readonly Line _bar2 = CreateBar();
    private readonly Line _bar3 = CreateBar();
    private readonly AbsoluteLayout _visualRoot = CreateVisualRootLayout();

    private double _barThickness = 2.5;
    private double _barSpacing = 6;
    private double _contentWidth;
    private double _contentHeight;

    public HamburgerToggleButton()
    {
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) };
        Padding = new Thickness(8);
        BackgroundColor = Colors.Transparent;
        StrokeThickness = 0;

        _visualRoot.Add(_bar1);
        _visualRoot.Add(_bar2);
        _visualRoot.Add(_bar3);

        Content = _visualRoot;

        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);

        ApplyPalette();
        UpdateBarLayout(false);
    }

    private static AbsoluteLayout CreateVisualRootLayout()
    {
        return new AbsoluteLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    public event EventHandler? Checked;
    public event EventHandler? Unchecked;

    #region Bindable properties

    public static readonly BindableProperty AnimationDurationProperty =
        BindableProperty.Create(
            nameof(AnimationDuration),
            typeof(uint),
            typeof(HamburgerToggleButton),
            DefaultAnimationDuration);

    public uint AnimationDuration
    {
        get => (uint)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(HamburgerToggleButton),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsCheckedChanged);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(HamburgerToggleButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(HamburgerToggleButton));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty PressedOpacityProperty =
        BindableProperty.Create(
            nameof(PressedOpacity),
            typeof(double),
            typeof(HamburgerToggleButton),
            0.85);

    public double PressedOpacity
    {
        get => (double)GetValue(PressedOpacityProperty);
        set => SetValue(PressedOpacityProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(HamburgerToggleButton),
            8f,
            propertyChanged: OnCornerRadiusChanged);

    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly BindableProperty BarColorProperty =
        BindableProperty.Create(
            nameof(BarColor),
            typeof(Color),
            typeof(HamburgerToggleButton),
            Colors.Black,
            propertyChanged: OnPaletteChanged);

    public Color BarColor
    {
        get => (Color)GetValue(BarColorProperty);
        set => SetValue(BarColorProperty, value);
    }

    public static readonly BindableProperty CheckedBarColorProperty =
        BindableProperty.Create(
            nameof(CheckedBarColor),
            typeof(Color),
            typeof(HamburgerToggleButton),
            Colors.Black,
            propertyChanged: OnPaletteChanged);

    public Color CheckedBarColor
    {
        get => (Color)GetValue(CheckedBarColorProperty);
        set => SetValue(CheckedBarColorProperty, value);
    }

    public static readonly BindableProperty DisabledBarColorProperty =
        BindableProperty.Create(
            nameof(DisabledBarColor),
            typeof(Color),
            typeof(HamburgerToggleButton),
            Colors.Gray,
            propertyChanged: OnPaletteChanged);

    public Color DisabledBarColor
    {
        get => (Color)GetValue(DisabledBarColorProperty);
        set => SetValue(DisabledBarColorProperty, value);
    }

    public static readonly BindableProperty BackgroundCheckedColorProperty =
        BindableProperty.Create(
            nameof(BackgroundCheckedColor),
            typeof(Color),
            typeof(HamburgerToggleButton),
            Colors.Transparent,
            propertyChanged: OnPaletteChanged);

    public Color BackgroundCheckedColor
    {
        get => (Color)GetValue(BackgroundCheckedColorProperty);
        set => SetValue(BackgroundCheckedColorProperty, value);
    }

    #endregion

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is HamburgerToggleButton button && newValue is float radius)
        {
            button.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(radius)
            };
        }
    }

    private static void OnPaletteChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is HamburgerToggleButton button)
        {
            button.ApplyPalette();
        }
    }

    private static Line CreateBar()
    {
        return new Line
        {
            StrokeLineCap = PenLineCap.Round,
            StrokeLineJoin = PenLineJoin.Round,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };
    }

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is HamburgerToggleButton button && newValue is bool isChecked)
        {
            if (isChecked)
            {
                button.Checked?.Invoke(button, EventArgs.Empty);
            }
            else
            {
                button.Unchecked?.Invoke(button, EventArgs.Empty);
            }

            VisualStateManager.GoToState(button, isChecked ? "Checked" : "Unchecked");
            button.ApplyPalette();
            button.UpdateBarLayout(true);
        }
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        var cmd = Command;
        var param = CommandParameter;

        var originalOpacity = Opacity;
        Opacity = PressedOpacity;
        await this.FadeTo(originalOpacity, 90, Easing.CubicOut);

        IsChecked = !IsChecked;

        if (cmd?.CanExecute(param) == true)
        {
            cmd.Execute(param);
        }
    }

    private void ApplyPalette()
    {
        var barColor = IsEnabled
            ? (IsChecked ? CheckedBarColor : BarColor)
            : DisabledBarColor;

        _bar1.Stroke = barColor;
        _bar2.Stroke = barColor;
        _bar3.Stroke = barColor;

        BackgroundColor = IsChecked ? BackgroundCheckedColor : Colors.Transparent;
        Opacity = IsEnabled ? 1d : 0.38;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        var contentWidth = Math.Max(0, width - Padding.HorizontalThickness);
        var contentHeight = Math.Max(0, height - Padding.VerticalThickness);

        if (Math.Abs(_contentWidth - contentWidth) < 0.5 && Math.Abs(_contentHeight - contentHeight) < 0.5)
        {
            return;
        }

        _contentWidth = contentWidth;
        _contentHeight = contentHeight;
        UpdateBarLayout(false);
    }

    private void UpdateBarLayout(bool animate)
    {
        _bar1.AbortAnimation("HamburgerAnim");
        _bar2.AbortAnimation("HamburgerAnim");
        _bar3.AbortAnimation("HamburgerAnim");

        if (_contentWidth <= 0 || _contentHeight <= 0)
        {
            return;
        }

        var size = Math.Min(_contentWidth, _contentHeight);
        _barThickness = Math.Max(MinBarThickness, size * 0.08);
        var inset = Math.Max(MinBarInset, _barThickness);
        _barSpacing = Math.Min((_contentHeight - 2 * inset) / 4, _contentHeight * 0.2);

        var xLeft = inset;
        var xRight = _contentWidth - inset;
        var xMid = _contentWidth / 2;
        var yTop = inset;
        var yBottom = _contentHeight - inset;
        var yMid = _contentHeight / 2;

        _bar1.StrokeThickness = _barThickness;
        _bar2.StrokeThickness = _barThickness;
        _bar3.StrokeThickness = _barThickness;

        SetLineBounds(_bar1);
        SetLineBounds(_bar2);
        SetLineBounds(_bar3);

        if (IsChecked)
        {
            var topTarget = new LinePoints(xLeft, yMid, xMid, yTop);
            var midTarget = new LinePoints(xLeft, yMid, xRight, yMid);
            var bottomTarget = new LinePoints(xLeft, yMid, xMid, yBottom);

            if (animate)
            {
                AnimateLines(topTarget, midTarget, bottomTarget);
            }
            else
            {
                ApplyLinePoints(_bar1, topTarget);
                ApplyLinePoints(_bar2, midTarget);
                ApplyLinePoints(_bar3, bottomTarget);
            }
        }
        else
        {
            var topTarget = new LinePoints(xLeft, yMid - _barSpacing, xRight, yMid - _barSpacing);
            var midTarget = new LinePoints(xLeft, yMid, xRight, yMid);
            var bottomTarget = new LinePoints(xLeft, yMid + _barSpacing, xRight, yMid + _barSpacing);

            if (animate)
            {
                AnimateLines(topTarget, midTarget, bottomTarget);
            }
            else
            {
                ApplyLinePoints(_bar1, topTarget);
                ApplyLinePoints(_bar2, midTarget);
                ApplyLinePoints(_bar3, bottomTarget);
            }
        }
    }

    private void SetLineBounds(Line line)
    {
        AbsoluteLayout.SetLayoutBounds(line, new Rect(0, 0, _contentWidth, _contentHeight));
        AbsoluteLayout.SetLayoutFlags(line, AbsoluteLayoutFlags.None);
    }

    private void AnimateLines(LinePoints topTarget, LinePoints midTarget, LinePoints bottomTarget)
    {
        var anim = new Animation();
        AddLineAnimation(anim, _bar1, topTarget);
        AddLineAnimation(anim, _bar2, midTarget);
        AddLineAnimation(anim, _bar3, bottomTarget);
        anim.Commit(this, "HamburgerAnim", 16, AnimationDuration, Easing.CubicOut);
    }

    private static void AddLineAnimation(Animation anim, Line line, LinePoints target)
    {
        anim.Add(0, 1, new Animation(v => line.X1 = v, line.X1, target.X1, Easing.CubicOut));
        anim.Add(0, 1, new Animation(v => line.Y1 = v, line.Y1, target.Y1, Easing.CubicOut));
        anim.Add(0, 1, new Animation(v => line.X2 = v, line.X2, target.X2, Easing.CubicOut));
        anim.Add(0, 1, new Animation(v => line.Y2 = v, line.Y2, target.Y2, Easing.CubicOut));
    }

    private static void ApplyLinePoints(Line line, LinePoints points)
    {
        line.X1 = points.X1;
        line.Y1 = points.Y1;
        line.X2 = points.X2;
        line.Y2 = points.Y2;
    }

    private readonly record struct LinePoints(double X1, double Y1, double X2, double Y2);
}
