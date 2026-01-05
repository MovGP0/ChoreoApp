namespace MaterialDesignThemes.Maui;

public static class RadioButtonAssist
{
    private const double DefaultRadioButtonSize = 18.0;
    private const string CheckAnimationName = "MaterialDesignRadioButtonPulse";

    public static readonly BindableProperty RadioButtonSizeProperty =
        BindableProperty.CreateAttached(
            "RadioButtonSize",
            typeof(double),
            typeof(RadioButtonAssist),
            DefaultRadioButtonSize);

    public static double GetRadioButtonSize(RadioButton element) =>
        (double)element.GetValue(RadioButtonSizeProperty);

    public static void SetRadioButtonSize(RadioButton element, double checkBoxSize) =>
        element.SetValue(RadioButtonSizeProperty, checkBoxSize);

    public static readonly BindableProperty AnimateOnCheckedProperty =
        BindableProperty.CreateAttached(
            "AnimateOnChecked",
            typeof(bool),
            typeof(RadioButtonAssist),
            false,
            propertyChanged: OnAnimateOnCheckedChanged);

    public static bool GetAnimateOnChecked(BindableObject element) =>
        (bool)element.GetValue(AnimateOnCheckedProperty);

    public static void SetAnimateOnChecked(BindableObject element, bool value) =>
        element.SetValue(AnimateOnCheckedProperty, value);

    private static void OnAnimateOnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not RadioButton radioButton)
        {
            return;
        }

        if ((bool)newValue)
        {
            radioButton.CheckedChanged -= OnCheckedChanged;
            radioButton.CheckedChanged += OnCheckedChanged;
        }
        else
        {
            radioButton.CheckedChanged -= OnCheckedChanged;
        }
    }

    private static void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is not RadioButton radioButton)
        {
            return;
        }

        if (!e.Value)
        {
            return;
        }

        radioButton.AbortAnimation(CheckAnimationName);

        const double startScale = 1.0;
        const double endScale = 0.92;
        const uint duration = 140;

        var animation = new Animation();
        animation.Add(
            0,
            0.5,
            new Animation(value => radioButton.Scale = value, startScale, endScale, Easing.CubicOut));
        animation.Add(
            0.5,
            1,
            new Animation(value => radioButton.Scale = value, endScale, startScale, Easing.CubicOut));

        animation.Commit(radioButton, CheckAnimationName, 16, duration);
    }
}
