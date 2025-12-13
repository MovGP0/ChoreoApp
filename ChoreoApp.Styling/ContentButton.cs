using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using ContentPropertyAttribute = Microsoft.Maui.Controls.ContentPropertyAttribute;

namespace ChoreoApp.Styling;

/// <summary>
/// A simple MAUI button-like control that supports arbitrary content (e.g., PackIcon + text).
/// Implements Command/CommandParameter and Clicked event. Uses a <see cref="Border"/> host.
/// </summary>
[ContentProperty(nameof(Content))]
public sealed class ContentButton : Border
{
    private readonly TapGestureRecognizer _tap;

    public ContentButton()
    {
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) };
        Padding = new Thickness(12, 10);
        BackgroundColor = Colors.Transparent;
        StrokeThickness = 0;

        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);
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

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        var cmd = Command;
        var param = CommandParameter;

        // brief visual feedback
        var originalOpacity = Opacity;
        Opacity = PressedOpacity;
        await this.FadeTo(originalOpacity, 90, Easing.CubicOut);

        if (cmd?.CanExecute(param) == true)
        {
            cmd.Execute(param);
        }

        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
