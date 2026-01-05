namespace MaterialDesignThemes.Maui;

/// <summary>
/// Provides shorthand to initialize a new <see cref="SnackbarMessageQueue"/> for a <see cref="Snackbar"/>.
/// </summary>
public sealed class MessageQueueExtension : IMarkupExtension<SnackbarMessageQueue>
{
    public SnackbarMessageQueue ProvideValue(IServiceProvider serviceProvider) => new();

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
