namespace MaterialDesignThemes.Maui;

public class ColorAdjustment : IMarkupExtension
{
    public float DesiredContrastRatio { get; set; } = 4.5f;

    public Contrast Contrast { get; set; } = Contrast.Medium;

    public ColorSelection Colors { get; set; } = ColorSelection.All;

    public object ProvideValue(IServiceProvider serviceProvider) => this;
}
