namespace MaterialDesignThemes.Maui;

public static class CheckBoxAssist
{
    private const double DefaultCheckBoxSize = 18.0;
    private const string CheckAnimationName = "MaterialDesignCheckBoxPulse";

    public static readonly BindableProperty CheckBoxSizeProperty =
        BindableProperty.CreateAttached(
            "CheckBoxSize",
            typeof(double),
            typeof(CheckBoxAssist),
            DefaultCheckBoxSize);

    public static double GetCheckBoxSize(CheckBox element) =>
        (double)element.GetValue(CheckBoxSizeProperty);

    public static void SetCheckBoxSize(CheckBox element, double checkBoxSize) =>
        element.SetValue(CheckBoxSizeProperty, checkBoxSize);

    public static readonly BindableProperty AnimateOnCheckedProperty =
        BindableProperty.CreateAttached(
            "AnimateOnChecked",
            typeof(bool),
            typeof(CheckBoxAssist),
            false,
            propertyChanged: OnAnimateOnCheckedChanged);

    public static bool GetAnimateOnChecked(BindableObject element) =>
        (bool)element.GetValue(AnimateOnCheckedProperty);

    public static void SetAnimateOnChecked(BindableObject element, bool value) =>
        element.SetValue(AnimateOnCheckedProperty, value);

    private static void OnAnimateOnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CheckBox checkBox)
        {
            return;
        }

        if ((bool)newValue)
        {
            checkBox.CheckedChanged -= OnCheckedChanged;
            checkBox.CheckedChanged += OnCheckedChanged;
        }
        else
        {
            checkBox.CheckedChanged -= OnCheckedChanged;
        }
    }

    private static void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox checkBox)
        {
            return;
        }

        checkBox.AbortAnimation(CheckAnimationName);

        const double startScale = 1.0;
        const double endScale = 0.92;
        const uint duration = 140;

        var animation = new Animation();
        animation.Add(
            0,
            0.5,
            new Animation(value => checkBox.Scale = value, startScale, endScale, Easing.CubicOut));
        animation.Add(
            0.5,
            1,
            new Animation(value => checkBox.Scale = value, endScale, startScale, Easing.CubicOut));

        animation.Commit(checkBox, CheckAnimationName, 16, duration);
    }
}
