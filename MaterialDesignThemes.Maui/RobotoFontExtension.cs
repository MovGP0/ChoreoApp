namespace MaterialDesignThemes.Maui;

public sealed class RobotoFontExtension : IMarkupExtension
{
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return "Roboto-Regular";
    }
}
