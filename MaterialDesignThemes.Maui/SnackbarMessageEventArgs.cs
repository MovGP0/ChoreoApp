namespace MaterialDesignThemes.Maui;

public sealed class SnackbarMessageEventArgs(SnackbarMessage message) : EventArgs
{
    public SnackbarMessage Message { get; } = message;
}
