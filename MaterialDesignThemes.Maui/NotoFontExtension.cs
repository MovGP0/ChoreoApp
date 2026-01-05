namespace MaterialDesignThemes.Maui;

public sealed class NotoFontExtension : IMarkupExtension
{
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return "NotoSans-Regular";
    }
}
