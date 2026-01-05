namespace MaterialDesignThemes.Maui;

/// <summary>
/// Allows transitions to be disabled where supported.
/// </summary>
public static class TransitionAssist
{
    /// <summary>
    /// Allows transitions to be disabled where supported.
    /// </summary>
    public static readonly BindableProperty DisableTransitionsProperty = BindableProperty.CreateAttached(
        "DisableTransitions",
        typeof(bool),
        typeof(TransitionAssist),
        false);

    /// <summary>
    /// Allows transitions to be disabled where supported.
    /// </summary>
    public static void SetDisableTransitions(BindableObject element, bool value)
    {
        element.SetValue(DisableTransitionsProperty, value);
    }

    /// <summary>
    /// Allows transitions to be disabled where supported.
    /// </summary>
    public static bool GetDisableTransitions(BindableObject element)
    {
        return (bool)element.GetValue(DisableTransitionsProperty);
    }
}
