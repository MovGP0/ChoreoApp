using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

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

    public ContentButton()
    {
        _border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
            Padding = new Thickness(12, 10),
            BackgroundColor = Colors.Transparent,
            StrokeThickness = 0
        };

        _border.SetBinding(Border.ContentProperty, new Binding(nameof(ButtonContent), source: this));
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
