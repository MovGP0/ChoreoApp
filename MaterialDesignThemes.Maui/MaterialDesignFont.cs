namespace MaterialDesignThemes.Maui;

public sealed class MaterialDesignFontExtension : IMarkupExtension<string>
{
    private const string DefaultFontAlias = "Roboto";

    public string ProvideValue(IServiceProvider serviceProvider) => DefaultFontAlias;

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
