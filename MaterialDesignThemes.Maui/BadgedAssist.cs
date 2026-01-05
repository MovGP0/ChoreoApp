namespace MaterialDesignThemes.Maui;

public static class BadgedAssist
{
    public static readonly BindableProperty BadgeProperty =
        BindableProperty.CreateAttached(
            "Badge",
            typeof(object),
            typeof(BadgedAssist),
            null);

    public static object? GetBadge(BindableObject element)
    {
        return element.GetValue(BadgeProperty);
    }

    public static void SetBadge(BindableObject element, object? value)
    {
        element.SetValue(BadgeProperty, value);
    }

    public static readonly BindableProperty BadgeBackgroundProperty =
        BindableProperty.CreateAttached(
            "BadgeBackground",
            typeof(Color),
            typeof(BadgedAssist),
            null);

    public static Color? GetBadgeBackground(BindableObject element)
    {
        return (Color?)element.GetValue(BadgeBackgroundProperty);
    }

    public static void SetBadgeBackground(BindableObject element, Color? value)
    {
        element.SetValue(BadgeBackgroundProperty, value);
    }

    public static readonly BindableProperty BadgeForegroundProperty =
        BindableProperty.CreateAttached(
            "BadgeForeground",
            typeof(Color),
            typeof(BadgedAssist),
            null);

    public static Color? GetBadgeForeground(BindableObject element)
    {
        return (Color?)element.GetValue(BadgeForegroundProperty);
    }

    public static void SetBadgeForeground(BindableObject element, Color? value)
    {
        element.SetValue(BadgeForegroundProperty, value);
    }

    public static readonly BindableProperty BadgePlacementModeProperty =
        BindableProperty.CreateAttached(
            "BadgePlacementMode",
            typeof(BadgePlacementMode),
            typeof(BadgedAssist),
            BadgePlacementMode.TopRight);

    public static BadgePlacementMode GetBadgePlacementMode(BindableObject element)
    {
        return (BadgePlacementMode)element.GetValue(BadgePlacementModeProperty);
    }

    public static void SetBadgePlacementMode(BindableObject element, BadgePlacementMode value)
    {
        element.SetValue(BadgePlacementModeProperty, value);
    }

    public static readonly BindableProperty IsMiniBadgeProperty =
        BindableProperty.CreateAttached(
            "IsMiniBadge",
            typeof(bool),
            typeof(BadgedAssist),
            false);

    public static bool GetIsMiniBadge(BindableObject element)
    {
        return (bool)element.GetValue(IsMiniBadgeProperty);
    }

    public static void SetIsMiniBadge(BindableObject element, bool value)
    {
        element.SetValue(IsMiniBadgeProperty, value);
    }
}
