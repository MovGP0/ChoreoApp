namespace MaterialDesignThemes.Maui;

public static class PasswordBoxAssist
{
    public static readonly BindableProperty PasswordMaskedIconProperty =
        BindableProperty.CreateAttached(
            "PasswordMaskedIcon",
            typeof(PackIconKind),
            typeof(PasswordBoxAssist),
            PackIconKind.EyeOff);

    public static void SetPasswordMaskedIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(PasswordMaskedIconProperty, value);

    public static PackIconKind GetPasswordMaskedIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(PasswordMaskedIconProperty);

    public static readonly BindableProperty PasswordRevealedIconProperty =
        BindableProperty.CreateAttached(
            "PasswordRevealedIcon",
            typeof(PackIconKind),
            typeof(PasswordBoxAssist),
            PackIconKind.Eye);

    public static void SetPasswordRevealedIcon(BindableObject element, PackIconKind value) =>
        element.SetValue(PasswordRevealedIconProperty, value);

    public static PackIconKind GetPasswordRevealedIcon(BindableObject element) =>
        (PackIconKind)element.GetValue(PasswordRevealedIconProperty);

    public static readonly BindableProperty IsPasswordRevealedProperty =
        BindableProperty.CreateAttached(
            "IsPasswordRevealed",
            typeof(bool),
            typeof(PasswordBoxAssist),
            false,
            propertyChanged: OnIsPasswordRevealedChanged);

    public static void SetIsPasswordRevealed(BindableObject element, bool value) =>
        element.SetValue(IsPasswordRevealedProperty, value);

    public static bool GetIsPasswordRevealed(BindableObject element) =>
        (bool)element.GetValue(IsPasswordRevealedProperty);

    public static readonly BindableProperty IsRevealButtonTabStopProperty =
        BindableProperty.CreateAttached(
            "IsRevealButtonTabStop",
            typeof(bool),
            typeof(PasswordBoxAssist),
            true);

    public static void SetIsRevealButtonTabStop(BindableObject element, bool value) =>
        element.SetValue(IsRevealButtonTabStopProperty, value);

    public static bool GetIsRevealButtonTabStop(BindableObject element) =>
        (bool)element.GetValue(IsRevealButtonTabStopProperty);

    public static readonly BindableProperty PasswordProperty =
        BindableProperty.CreateAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxAssist),
            default(string),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnPasswordChanged);

    public static void SetPassword(BindableObject element, string? value) =>
        element.SetValue(PasswordProperty, value);

    public static string? GetPassword(BindableObject element) =>
        (string?)element.GetValue(PasswordProperty);

    private static readonly BindableProperty IsChangingProperty =
        BindableProperty.CreateAttached(
            "IsChanging",
            typeof(bool),
            typeof(PasswordBoxAssist),
            false);

    private static void SetIsChanging(BindableObject element, bool value) =>
        element.SetValue(IsChangingProperty, value);

    private static bool GetIsChanging(BindableObject element) =>
        (bool)element.GetValue(IsChangingProperty);

    internal static readonly BindableProperty SuppressBindingGuardProperty =
        BindableProperty.CreateAttached(
            "SuppressBindingGuard",
            typeof(bool),
            typeof(PasswordBoxAssist),
            false);

    internal static void SetSuppressBindingGuard(BindableObject element, bool value) =>
        element.SetValue(SuppressBindingGuardProperty, value);

    internal static bool GetSuppressBindingGuard(BindableObject element) =>
        (bool)element.GetValue(SuppressBindingGuardProperty);

    private static void OnIsPasswordRevealedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Entry entry)
        {
            entry.IsPassword = !(bool)newValue;
        }
    }

    private static void OnPasswordChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Entry entry)
        {
            return;
        }

        entry.TextChanged -= OnEntryTextChanged;
        entry.TextChanged += OnEntryTextChanged;

        if (GetIsChanging(entry))
        {
            return;
        }

        SetIsChanging(entry, true);
        entry.Text = newValue as string ?? string.Empty;
        SetIsChanging(entry, false);
    }

    private static void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
        {
            return;
        }

        SetIsChanging(entry, true);
        SetPassword(entry, e.NewTextValue);
        SetIsChanging(entry, false);
    }
}
