namespace MaterialDesignThemes.Maui;

public sealed class DrawerClosingEventArgs(DrawerDock dock) : EventArgs
{
    public DrawerDock Dock { get; } = dock;

    public bool IsCancelled { get; private set; }

    public void Cancel() => IsCancelled = true;
}
