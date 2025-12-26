using Application = Microsoft.Maui.Controls.Application;

namespace MaterialDesignThemes.Maui;

public enum Elevation
{
    Dp0,
    Dp1,
    Dp2,
    Dp3,
    Dp4,
    Dp5,
    Dp6,
    Dp7,
    Dp8,
    Dp12,
    Dp16,
    Dp24
}

internal static class ElevationInfo
{
    private static readonly IReadOnlyDictionary<Elevation, string> ShadowKeys = new Dictionary<Elevation, string>
    {
        { Elevation.Dp1, MaterialDesignStyleKey.MaterialDesignElevationShadow1 },
        { Elevation.Dp2, MaterialDesignStyleKey.MaterialDesignElevationShadow2 },
        { Elevation.Dp3, MaterialDesignStyleKey.MaterialDesignElevationShadow3 },
        { Elevation.Dp4, MaterialDesignStyleKey.MaterialDesignElevationShadow4 },
        { Elevation.Dp5, MaterialDesignStyleKey.MaterialDesignElevationShadow5 },
        { Elevation.Dp6, MaterialDesignStyleKey.MaterialDesignElevationShadow6 },
        { Elevation.Dp7, MaterialDesignStyleKey.MaterialDesignElevationShadow7 },
        { Elevation.Dp8, MaterialDesignStyleKey.MaterialDesignElevationShadow8 },
        { Elevation.Dp12, MaterialDesignStyleKey.MaterialDesignElevationShadow12 },
        { Elevation.Dp16, MaterialDesignStyleKey.MaterialDesignElevationShadow16 },
        { Elevation.Dp24, MaterialDesignStyleKey.MaterialDesignElevationShadow24 }
    };

    public static Shadow? GetShadow(Elevation elevation)
    {
        if (elevation == Elevation.Dp0)
        {
            return null;
        }

        if (!ShadowKeys.TryGetValue(elevation, out var key))
        {
            return null;
        }

        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Shadow shadow)
        {
            return shadow;
        }

        return null;
    }
}

/// <summary>
/// Attached property helper for Material Design elevation in MAUI.
/// </summary>
public static class ElevationAssist
{
    public static readonly BindableProperty ElevationProperty = BindableProperty.CreateAttached(
        "Elevation",
        typeof(Elevation),
        typeof(ElevationAssist),
        default(Elevation),
        propertyChanged: OnElevationChanged);

    public static void SetElevation(BindableObject element, Elevation value) =>
        element.SetValue(ElevationProperty, value);

    public static Elevation GetElevation(BindableObject element) =>
        (Elevation)element.GetValue(ElevationProperty);

    public static Shadow? GetShadow(Elevation elevation) =>
        ElevationInfo.GetShadow(elevation);

    private static void OnElevationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is VisualElement element && newValue is Elevation elevation)
        {
            element.Shadow = ElevationInfo.GetShadow(elevation);
        }
    }
}
