using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Minimal toggle button for MAUI (Border-based) with hamburger-to-X animation akin to MaterialDesign.
/// Exposes IsChecked, Checked/Unchecked, Command, and palette properties.
/// </summary>
public sealed class HamburgerToggleButton : Border
{
    private readonly TapGestureRecognizer _tap = new();
    private readonly BoxView _bar1 = CreateBar();
    private readonly BoxView _bar2 = CreateBar();
    private readonly BoxView _bar3 = CreateBar();
    private readonly Grid _visualRoot = CreateVisualRootGrid();

    private const uint AnimationDuration = 160;
    private const double BarHeightDefault = 2.5;
    private const double BarWidthDefault = 22;
    private const double BarSpacing = 6; // distance between bars (center to center)

    public HamburgerToggleButton()
    {
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) };
        Padding = new Thickness(8);
        BackgroundColor = Colors.Transparent;
        StrokeThickness = 0;

        _visualRoot.Add(_bar1);
        _visualRoot.Add(_bar2, 0, 1);
        _visualRoot.Add(_bar3, 0, 2);

        Content = _visualRoot;

        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);

        ApplyPalette();
        ResetTransforms();
    }

    private static Grid CreateVisualRootGrid()
    {
        return new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            },
            RowSpacing = 4,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    public event EventHandler? Checked;
    public event EventHandler? Unchecked;

    #region Bindable properties

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
            typeof(HamburgerToggleButton),
            default(ICommand));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(HamburgerToggleButton),
            null);

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

    private static BoxView CreateBar()
    {
        return new BoxView
        {
            HeightRequest = BarHeightDefault,
            WidthRequest = BarWidthDefault,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            CornerRadius = new CornerRadius(1.25)
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
            button.RunHamburgerAnimation(isChecked);
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

        _bar1.Color = barColor;
        _bar2.Color = barColor;
        _bar3.Color = barColor;

        BackgroundColor = IsChecked ? BackgroundCheckedColor : Colors.Transparent;
        Opacity = IsEnabled ? 1d : 0.38;
    }

    private void ResetTransforms()
    {
        _bar1.Rotation = 0;
        _bar3.Rotation = 0;
        _bar2.Opacity = 1;
        _bar1.TranslationY = -BarSpacing;
        _bar2.TranslationY = 0;
        _bar3.TranslationY = BarSpacing;
    }

    private void RunHamburgerAnimation(bool isChecked)
    {
        _bar1.AbortAnimation("HamburgerAnim");
        _bar2.AbortAnimation("HamburgerAnim");
        _bar3.AbortAnimation("HamburgerAnim");

        var translateUp = -BarSpacing;
        var translateDown = BarSpacing;

        if (isChecked)
        {
            var anim = new Animation();
            anim.Add(0, 1, new Animation(v => _bar1.TranslationY = v, _bar1.TranslationY, 0, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar3.TranslationY = v, _bar3.TranslationY, 0, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar1.Rotation = v, _bar1.Rotation, 45, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar3.Rotation = v, _bar3.Rotation, -45, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar2.Opacity = v, _bar2.Opacity, 0, Easing.CubicOut));
            anim.Commit(this, "HamburgerAnim", 16, AnimationDuration, Easing.CubicOut);
        }
        else
        {
            var anim = new Animation();
            anim.Add(0, 1, new Animation(v => _bar1.TranslationY = v, _bar1.TranslationY, translateUp, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar3.TranslationY = v, _bar3.TranslationY, translateDown, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar1.Rotation = v, _bar1.Rotation, 0, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar3.Rotation = v, _bar3.Rotation, 0, Easing.CubicOut));
            anim.Add(0, 1, new Animation(v => _bar2.Opacity = v, _bar2.Opacity, 1, Easing.CubicOut));
            anim.Commit(this, "HamburgerAnim", 16, AnimationDuration, Easing.CubicOut);
        }
    }
}
