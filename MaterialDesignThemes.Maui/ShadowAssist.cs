namespace MaterialDesignThemes.Maui;

internal sealed record ShadowLocalInfo(double StandardOpacity);

public static class ShadowAssist
{
    private static readonly BindablePropertyKey LocalInfoPropertyKey = BindableProperty.CreateAttachedReadOnly(
        "LocalInfo",
        typeof(ShadowLocalInfo),
        typeof(ShadowAssist),
        null);

    private static void SetLocalInfo(BindableObject element, ShadowLocalInfo? value)
    {
        element.SetValue(LocalInfoPropertyKey, value);
    }

    private static ShadowLocalInfo? GetLocalInfo(BindableObject element)
    {
        return (ShadowLocalInfo?)element.GetValue(LocalInfoPropertyKey.BindableProperty);
    }

    public static readonly BindableProperty DarkenProperty = BindableProperty.CreateAttached(
        "Darken",
        typeof(bool),
        typeof(ShadowAssist),
        false,
        propertyChanged: DarkenPropertyChanged);

    public static void SetDarken(BindableObject element, bool value)
    {
        element.SetValue(DarkenProperty, value);
    }

    public static bool GetDarken(BindableObject element)
    {
        return (bool)element.GetValue(DarkenProperty);
    }

    public static readonly BindableProperty CacheModeProperty = BindableProperty.CreateAttached(
        "CacheMode",
        typeof(object),
        typeof(ShadowAssist),
        null);

    public static void SetCacheMode(BindableObject element, object? value)
    {
        element.SetValue(CacheModeProperty, value);
    }

    public static object? GetCacheMode(BindableObject element)
    {
        return element.GetValue(CacheModeProperty);
    }

    public static readonly BindableProperty ShadowAnimationDurationProperty = BindableProperty.CreateAttached(
        "ShadowAnimationDuration",
        typeof(TimeSpan),
        typeof(ShadowAssist),
        TimeSpan.FromMilliseconds(180));

    public static TimeSpan GetShadowAnimationDuration(BindableObject element)
    {
        return (TimeSpan)element.GetValue(ShadowAnimationDurationProperty);
    }

    public static void SetShadowAnimationDuration(BindableObject element, TimeSpan value)
    {
        element.SetValue(ShadowAnimationDurationProperty, value);
    }

    private static void DarkenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not VisualElement element)
        {
            return;
        }

        var shadow = element.Shadow;
        if (shadow is null)
        {
            return;
        }

        shadow = EnsureLocalShadow(element, shadow);

        if (newValue is not bool shouldDarken)
        {
            return;
        }

        double? targetOpacity;
        if (shouldDarken)
        {
            SetLocalInfo(bindable, new ShadowLocalInfo(shadow.Opacity));
            targetOpacity = 1.0;
        }
        else
        {
            var localInfo = GetLocalInfo(bindable);
            if (localInfo is null)
            {
                return;
            }

            targetOpacity = localInfo.StandardOpacity;
        }

        AnimateOpacity(element, shadow, targetOpacity.Value);
    }

    private static Shadow EnsureLocalShadow(VisualElement element, Shadow shadow)
    {
        var localShadow = new Shadow
        {
            Brush = shadow.Brush,
            Offset = shadow.Offset,
            Radius = shadow.Radius,
            Opacity = shadow.Opacity
        };

        element.Shadow = localShadow;
        return localShadow;
    }

    private static void AnimateOpacity(VisualElement element, Shadow shadow, double toOpacity)
    {
        var duration = GetShadowAnimationDuration(element);
        var length = (uint)Math.Max(0, duration.TotalMilliseconds);
        var fromOpacity = shadow.Opacity;

        element.AbortAnimation("ShadowAssistOpacity");

        var animation = new Animation(
            v => shadow.Opacity = (float)v,
            fromOpacity,
            toOpacity,
            Easing.CubicOut);

        animation.Commit(element, "ShadowAssistOpacity", length: length);
    }
}
