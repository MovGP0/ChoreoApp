namespace MaterialDesignThemes.Maui;

[Obsolete("This class is obsolete and will be removed in a future version. Please use the TextFieldAssist equivalents instead.")]
public static class TimePickerAssist
{
    public static readonly BindableProperty OutlinedBorderInactiveThicknessProperty = BindableProperty.CreateAttached(
        "OutlinedBorderInactiveThickness",
        typeof(Thickness),
        typeof(TimePickerAssist),
        new Thickness(1));

    public static void SetOutlinedBorderInactiveThickness(BindableObject element, Thickness value)
        => element.SetValue(OutlinedBorderInactiveThicknessProperty, value);

    public static Thickness GetOutlinedBorderInactiveThickness(BindableObject element)
        => (Thickness)element.GetValue(OutlinedBorderInactiveThicknessProperty);

    public static readonly BindableProperty OutlinedBorderActiveThicknessProperty = BindableProperty.CreateAttached(
        "OutlinedBorderActiveThickness",
        typeof(Thickness),
        typeof(TimePickerAssist),
        new Thickness(2));

    public static void SetOutlinedBorderActiveThickness(BindableObject element, Thickness value)
        => element.SetValue(OutlinedBorderActiveThicknessProperty, value);

    public static Thickness GetOutlinedBorderActiveThickness(BindableObject element)
        => (Thickness)element.GetValue(OutlinedBorderActiveThicknessProperty);
}
