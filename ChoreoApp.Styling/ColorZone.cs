using Microsoft.Maui.Controls.Shapes;
using Application = Microsoft.Maui.Controls.Application;

namespace ChoreoApp.Styling;

/// <summary>
/// A simple MAUI port of the MaterialDesign ColorZone. Sets its background (and exposes a
/// foreground resource) based on the selected <see cref="ColorZoneMode"/>.
/// </summary>
public sealed class ColorZone : Border
{
    private const string ForegroundResourceKey = "ColorZoneForegroundColor";

    public ColorZone()
    {
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0) };
        ApplyMode();
    }

    public static readonly BindableProperty ModeProperty =
        BindableProperty.Create(
            nameof(Mode),
            typeof(ColorZoneMode),
            typeof(ColorZone),
            ColorZoneMode.Standard,
            propertyChanged: OnModeChanged);

    public ColorZoneMode Mode
    {
        get => (ColorZoneMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(ColorZone),
            default(CornerRadius),
            propertyChanged: OnCornerRadiusChanged);

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ColorZone zone)
        {
            zone.ApplyMode();
        }
    }

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ColorZone zone && newValue is CornerRadius radius)
        {
            zone.StrokeShape = new RoundRectangle
            {
                CornerRadius = radius
            };
        }
    }

    private void ApplyMode()
    {
        var (background, foreground) = GetPaletteForMode(Mode);

        if (background != null)
        {
            Background = new SolidColorBrush(background);
        }

        if (foreground != null)
        {
            Resources[ForegroundResourceKey] = foreground;
        }
    }

    private static (Color? background, Color? foreground) GetPaletteForMode(ColorZoneMode mode)
    {
        Color? TryResource(string key)
        {
            if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color)
            {
                return color;
            }

            return null;
        }

        return mode switch
        {
            ColorZoneMode.Standard => (TryResource(MaterialDesignColorKey.Surface), TryResource(MaterialDesignColorKey.OnSurface)),
            ColorZoneMode.Inverted => (TryResource(MaterialDesignColorKey.OnSurface), TryResource(MaterialDesignColorKey.Surface)),
            ColorZoneMode.PrimaryLight => (TryResource(MaterialDesignColorKey.PrimaryContainer), TryResource(MaterialDesignColorKey.OnPrimaryContainer)),
            ColorZoneMode.PrimaryMid => (TryResource(MaterialDesignColorKey.Primary), TryResource(MaterialDesignColorKey.OnPrimary)),
            ColorZoneMode.PrimaryDark => (TryResource(MaterialDesignColorKey.PrimaryFixed), TryResource(MaterialDesignColorKey.OnPrimaryFixed)),
            ColorZoneMode.SecondaryLight => (TryResource(MaterialDesignColorKey.SecondaryContainer), TryResource(MaterialDesignColorKey.OnSecondaryContainer)),
            ColorZoneMode.SecondaryMid => (TryResource(MaterialDesignColorKey.Secondary), TryResource(MaterialDesignColorKey.OnSecondary)),
            ColorZoneMode.SecondaryDark => (TryResource(MaterialDesignColorKey.SecondaryFixed), TryResource(MaterialDesignColorKey.OnSecondaryFixed)),
            ColorZoneMode.Light => (TryResource(MaterialDesignColorKey.Surface), TryResource(MaterialDesignColorKey.OnSurface)),
            ColorZoneMode.Dark => (TryResource(MaterialDesignColorKey.SurfaceVariant), TryResource(MaterialDesignColorKey.OnSurfaceVariant)),
            ColorZoneMode.Custom => (null, null),
            _ => (null, null)
        };
    }
}
