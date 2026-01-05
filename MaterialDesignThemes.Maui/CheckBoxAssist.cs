namespace MaterialDesignThemes.Maui;

public static class CheckBoxAssist
{
    private const double DefaultCheckBoxSize = 18.0;

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
}
