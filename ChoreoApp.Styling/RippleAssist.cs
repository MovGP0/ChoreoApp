namespace ChoreoApp.Styling;

public static class RippleAssist
{
    public static readonly BindableProperty ClipToBoundsProperty = BindableProperty.CreateAttached(
        "ClipToBounds",
        typeof(bool),
        typeof(RippleAssist),
        true);

    public static void SetClipToBounds(BindableObject element, bool value)
    {
        element.SetValue(ClipToBoundsProperty, value);
    }

    public static bool GetClipToBounds(BindableObject element)
    {
        return (bool)element.GetValue(ClipToBoundsProperty);
    }

    public static readonly BindableProperty IsCenteredProperty = BindableProperty.CreateAttached(
        "IsCentered",
        typeof(bool),
        typeof(RippleAssist),
        false);

    public static void SetIsCentered(BindableObject element, bool value)
    {
        element.SetValue(IsCenteredProperty, value);
    }

    public static bool GetIsCentered(BindableObject element)
    {
        return (bool)element.GetValue(IsCenteredProperty);
    }

    public static readonly BindableProperty IsDisabledProperty = BindableProperty.CreateAttached(
        "IsDisabled",
        typeof(bool),
        typeof(RippleAssist),
        false);

    public static void SetIsDisabled(BindableObject element, bool value)
    {
        element.SetValue(IsDisabledProperty, value);
    }

    public static bool GetIsDisabled(BindableObject element)
    {
        return (bool)element.GetValue(IsDisabledProperty);
    }

    public static readonly BindableProperty RippleSizeMultiplierProperty = BindableProperty.CreateAttached(
        "RippleSizeMultiplier",
        typeof(double),
        typeof(RippleAssist),
        1.0);

    public static void SetRippleSizeMultiplier(BindableObject element, double value)
    {
        element.SetValue(RippleSizeMultiplierProperty, value);
    }

    public static double GetRippleSizeMultiplier(BindableObject element)
    {
        return (double)element.GetValue(RippleSizeMultiplierProperty);
    }

    public static readonly BindableProperty FeedbackProperty = BindableProperty.CreateAttached(
        "Feedback",
        typeof(Color),
        typeof(RippleAssist),
        null);

    public static void SetFeedback(BindableObject element, Color? value)
    {
        element.SetValue(FeedbackProperty, value);
    }

    public static Color? GetFeedback(BindableObject element)
    {
        return (Color?)element.GetValue(FeedbackProperty);
    }

    public static readonly BindableProperty RippleOnTopProperty = BindableProperty.CreateAttached(
        "RippleOnTop",
        typeof(bool),
        typeof(RippleAssist),
        false);

    public static void SetRippleOnTop(BindableObject element, bool value)
    {
        element.SetValue(RippleOnTopProperty, value);
    }

    public static bool GetRippleOnTop(BindableObject element)
    {
        return (bool)element.GetValue(RippleOnTopProperty);
    }
}
