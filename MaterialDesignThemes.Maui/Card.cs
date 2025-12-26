using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// A MAUI port of the Material Design card control using templates.
/// </summary>
[ContentProperty(nameof(Content))]
public sealed class Card : ContentView
{
    private const double DefaultUniformCornerRadius = 4.0;

    private static readonly BindablePropertyKey ContentClipPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(ContentClip),
            typeof(Geometry),
            typeof(Card),
            null);
    public static readonly BindableProperty ContentClipProperty =
        ContentClipPropertyKey.BindableProperty;

    public static readonly BindableProperty UniformCornerRadiusProperty =
        BindableProperty.Create(
            nameof(UniformCornerRadius),
            typeof(double),
            typeof(Card),
            DefaultUniformCornerRadius,
            propertyChanged: OnClipPropertyChanged);

    public double UniformCornerRadius
    {
        get => (double)GetValue(UniformCornerRadiusProperty);
        set => SetValue(UniformCornerRadiusProperty, value);
    }

    public static readonly BindableProperty ClipContentProperty =
        BindableProperty.Create(
            nameof(ClipContent),
            typeof(bool),
            typeof(Card),
            false,
            propertyChanged: OnClipPropertyChanged);

    public bool ClipContent
    {
        get => (bool)GetValue(ClipContentProperty);
        set => SetValue(ClipContentProperty, value);
    }

    public Geometry? ContentClip
    {
        get => (Geometry?)GetValue(ContentClipProperty);
        private set => SetValue(ContentClipPropertyKey, value);
    }

    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(Card),
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
            typeof(Card),
            0d);

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly BindableProperty ElevationProperty =
        BindableProperty.Create(
            nameof(Elevation),
            typeof(Elevation),
            typeof(Card),
            Elevation.Dp0);

    public Elevation Elevation
    {
        get => (Elevation)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        UpdateContentClip(width, height);
    }

    private static void OnClipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Card card)
        {
            card.UpdateContentClip(card.Width, card.Height);
        }
    }

    private void UpdateContentClip(double width, double height)
    {
        if (!ClipContent)
        {
            ContentClip = null;
            return;
        }

        if (width <= 0 || height <= 0)
        {
            return;
        }

        ContentClip = new RoundRectangleGeometry
        {
            Rect = new Rect(0, 0, width, height),
            CornerRadius = new CornerRadius(Math.Max(0, UniformCornerRadius))
        };
    }
}
