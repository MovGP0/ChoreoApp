namespace MaterialDesignThemes.Maui;

public static class RadioButtonAssist
{
    private const double DefaultRadioButtonSize = 18.0;

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
}
