using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Lightweight MAUI port of the MaterialDesignThemes PackIcon control.
/// </summary>
public class PackIcon : ContentView
{
    private static readonly Brush DefaultBrush = new SolidColorBrush(Colors.Black);
    private static readonly Brush DefaultBackgroundBrush = new SolidColorBrush(Colors.Transparent);

    private readonly Path _path;

    public PackIcon()
    {
        Background = DefaultBackgroundBrush;

        _path = new Path
        {
            Fill = DefaultBrush,
            Stroke = DefaultBackgroundBrush,
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

    public static readonly BindableProperty ForegroundColorProperty =
        BindableProperty.Create(
            nameof(ForegroundColor),
            typeof(Color),
            typeof(PackIcon),
            Colors.Black,
            propertyChanged: OnForegroundColorChanged);

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
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

    private static void OnForegroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PackIcon control && newValue is Color color)
        {
            control.Foreground = new SolidColorBrush(color);
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
