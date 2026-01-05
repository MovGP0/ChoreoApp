namespace MaterialDesignThemes.Maui;

[Obsolete("This class is obsolete and will be removed in a future version. Please use the TextFieldAssist equivalents instead. For OutlinedBorderInactiveThickness, simply use DatePicker.BorderThickness property instead.")]
public static class DatePickerAssist
{
    public static readonly BindableProperty OutlinedBorderInactiveThicknessProperty =
        BindableProperty.CreateAttached(
            "OutlinedBorderInactiveThickness",
            typeof(Thickness),
            typeof(DatePickerAssist),
            Constants.DefaultOutlinedBorderInactiveThickness);

    public static void SetOutlinedBorderInactiveThickness(BindableObject element, Thickness value) =>
        element.SetValue(OutlinedBorderInactiveThicknessProperty, value);

    public static Thickness GetOutlinedBorderInactiveThickness(BindableObject element) =>
        (Thickness)element.GetValue(OutlinedBorderInactiveThicknessProperty);

    public static readonly BindableProperty OutlinedBorderActiveThicknessProperty =
        BindableProperty.CreateAttached(
            "OutlinedBorderActiveThickness",
            typeof(Thickness),
            typeof(DatePickerAssist),
            Constants.DefaultOutlinedBorderActiveThickness);

    public static void SetOutlinedBorderActiveThickness(BindableObject element, Thickness value) =>
        element.SetValue(OutlinedBorderActiveThicknessProperty, value);

    public static Thickness GetOutlinedBorderActiveThickness(BindableObject element) =>
        (Thickness)element.GetValue(OutlinedBorderActiveThicknessProperty);
}
