namespace MaterialDesignThemes.Maui;

public sealed class ClockChoiceMadeEventArgs(ClockDisplayMode mode): EventArgs
{
    public ClockDisplayMode Mode { get; } = mode;
}
