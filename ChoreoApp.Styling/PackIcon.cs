using Microsoft.Maui.Graphics;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace ChoreoApp.Styling;

/// <summary>
/// Lightweight MAUI port of the MaterialDesignThemes PackIcon control.
/// </summary>
public class PackIcon : ContentView
{
    private static readonly Brush DefaultBrush = new SolidColorBrush(Colors.Black);

    private readonly Path _path;

    public PackIcon()
    {
        _path = new Path
        {
            Fill = DefaultBrush,
            Stroke = DefaultBrush,
            StrokeThickness = 0
        };

        Content = _path;

        HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Center;
        WidthRequest = 24;
        HeightRequest = 24;

        UpdateIcon();
        UpdateForeground();
    }

    public static readonly BindableProperty KindProperty =
        BindableProperty.Create(
            nameof(Kind),
            typeof(PackIconKind),
            typeof(PackIcon),
            default(PackIconKind),
            propertyChanged: OnKindChanged);

    public PackIconKind Kind
    {
        get => (PackIconKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.Create(
            nameof(Foreground),
            typeof(Brush),
            typeof(PackIcon),
            DefaultBrush,
            propertyChanged: OnForegroundChanged);

    public Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    private static void OnKindChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PackIcon control)
        {
            control.UpdateIcon();
        }
    }

    private static void OnForegroundChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PackIcon control)
        {
            control.UpdateForeground();
        }
    }

    private void UpdateIcon()
    {
        _path.Data = PackIconGeometryParser.Parse(Kind);
    }

    private void UpdateForeground()
    {
        _path.Fill = Foreground ?? DefaultBrush;
    }
}
