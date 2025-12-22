using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace ChoreoApp.Styling;

/// <summary>
/// A simple MAUI button-like control that supports arbitrary content (e.g., PackIcon + text).
/// Implements Command/CommandParameter and Clicked event. Uses a <see cref="Border"/> host.
/// </summary>
[ContentProperty(nameof(ButtonContent))]
public sealed class ContentButton : ContentView
{
    private readonly Border _border;
    private readonly TapGestureRecognizer _tap;
    private readonly PointerGestureRecognizer _pointer;
    private readonly ContentPresenter _contentPresenter;
    private readonly Ripple _ripple;

    public ContentButton()
    {
        _border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
            Padding = new Thickness(12, 10),
            BackgroundColor = Colors.Transparent,
            StrokeThickness = 0
        };

        _contentPresenter = new ContentPresenter();
        _contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding(nameof(ButtonContent), source: this));
        _contentPresenter.SetBinding(HorizontalOptionsProperty, new Binding(nameof(HorizontalContentAlignment), source: this));
        _contentPresenter.SetBinding(VerticalOptionsProperty, new Binding(nameof(VerticalContentAlignment), source: this));

        _ripple = new Ripple();
        _ripple.RippleContent = _contentPresenter;
        _ripple.SetBinding(RippleAssist.FeedbackProperty, new Binding("(styling:RippleAssist.Feedback)", source: this));
        _ripple.SetBinding(RippleAssist.IsCenteredProperty, new Binding("(styling:RippleAssist.IsCentered)", source: this));
        _ripple.SetBinding(RippleAssist.RippleSizeMultiplierProperty, new Binding("(styling:RippleAssist.RippleSizeMultiplier)", source: this));

        _border.Content = _ripple;
        _border.SetBinding(Border.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this));
        _border.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding), source: this));
        _border.SetBinding(Border.StrokeProperty, new Binding(nameof(Stroke), source: this));
        _border.SetBinding(Border.StrokeThicknessProperty, new Binding(nameof(StrokeThickness), source: this));

        Content = _border;

        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);

        _pointer = new PointerGestureRecognizer();
        _pointer.PointerEntered += OnPointerEntered;
        _pointer.PointerExited += OnPointerExited;
        GestureRecognizers.Add(_pointer);

        UpdateCornerRadius();
        UpdateEnabledState();
        UpdateRippleDisabledState();
    }

    public event EventHandler? Clicked;

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(ContentButton),
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
            typeof(ContentButton),
            null);

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty ButtonContentProperty =
        BindableProperty.Create(
            nameof(ButtonContent),
            typeof(View),
            typeof(ContentButton),
            null);

    public View? ButtonContent
    {
        get => (View?)GetValue(ButtonContentProperty);
        set => SetValue(ButtonContentProperty, value);
    }

    public static readonly BindableProperty HorizontalContentAlignmentProperty =
        BindableProperty.Create(
            nameof(HorizontalContentAlignment),
            typeof(LayoutOptions),
            typeof(ContentButton),
            LayoutOptions.Center);

    public LayoutOptions HorizontalContentAlignment
    {
        get => (LayoutOptions)GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly BindableProperty VerticalContentAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalContentAlignment),
            typeof(LayoutOptions),
            typeof(ContentButton),
            LayoutOptions.Center);

    public LayoutOptions VerticalContentAlignment
    {
        get => (LayoutOptions)GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.Create(
            nameof(Foreground),
            typeof(Color),
            typeof(ContentButton),
            null);

    public Color? Foreground
    {
        get => (Color?)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(ContentButton),
            14d);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(ContentButton),
            FontAttributes.None);

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(ContentButton),
            null);

    public string? FontFamily
    {
        get => (string?)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(ContentButton),
            null);

    public Brush? Stroke
    {
        get => (Brush?)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(
            nameof(StrokeThickness),
            typeof(double),
            typeof(ContentButton),
            0d);

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly BindableProperty PressedOpacityProperty =
        BindableProperty.Create(
            nameof(PressedOpacity),
            typeof(double),
            typeof(ContentButton),
            0.8);

    public double PressedOpacity
    {
        get => (double)GetValue(PressedOpacityProperty);
        set => SetValue(PressedOpacityProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(ContentButton),
            8f,
            propertyChanged: OnCornerRadiusChanged);

    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    protected override void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
        {
            UpdateEnabledState();
            UpdateRippleDisabledState();
        }
    }

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ContentButton button && newValue is float radius)
        {
            button.UpdateCornerRadius();
        }
    }

    private void UpdateCornerRadius()
    {
        _border.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(CornerRadius)
        };
    }

    private void UpdateEnabledState()
    {
        VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled");
    }

    private void UpdateRippleDisabledState()
    {
        RippleAssist.SetIsDisabled(_ripple, !IsEnabled);
    }

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        if (IsEnabled)
        {
            VisualStateManager.GoToState(this, "PointerOver");
        }
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        if (IsEnabled)
        {
            VisualStateManager.GoToState(this, "Normal");
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

        VisualStateManager.GoToState(this, "Pressed");

        // brief visual feedback
        var originalOpacity = Opacity;
        Opacity = PressedOpacity;
        await this.FadeTo(originalOpacity, 90, Easing.CubicOut);

        if (cmd?.CanExecute(param) == true)
        {
            cmd.Execute(param);
        }

        Clicked?.Invoke(this, EventArgs.Empty);

        VisualStateManager.GoToState(this, "Normal");
    }
}
