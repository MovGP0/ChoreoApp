namespace ChoreoApp.Styling;

public sealed class DrawerOpenedEventArgs(DrawerDock dock) : EventArgs
{
    public DrawerDock Dock { get; } = dock;
}
