using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using ContentPropertyAttribute = Microsoft.Maui.Controls.ContentPropertyAttribute;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Minimal toggle button for MAUI (no built-in ToggleButton). Derived from Border to allow full styling.
/// Exposes IsChecked, Checked/Unchecked events, and Command execution on toggle.
/// </summary>
[ContentProperty(nameof(Content))]
public sealed class ToogleButton : Border
{
    private readonly TapGestureRecognizer _tap;

    public ToogleButton()
    {
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) };
        Padding = new Thickness(8);
        BackgroundColor = Colors.Transparent;
        StrokeThickness = 0;

        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);
    }

    public event EventHandler? Checked;

    public event EventHandler? Unchecked;

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(ToogleButton),
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
            typeof(ToogleButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(ToogleButton));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty PressedOpacityProperty =
        BindableProperty.Create(
            nameof(PressedOpacity),
            typeof(double),
            typeof(ToogleButton),
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
            typeof(ToogleButton),
            8f,
            propertyChanged: OnCornerRadiusChanged);

    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ToogleButton button && newValue is float radius)
        {
            button.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(radius)
            };
        }
    }

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ToogleButton button && newValue is bool isChecked)
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
}
