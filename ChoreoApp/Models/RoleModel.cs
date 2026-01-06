namespace ChoreoApp.Models;

public sealed partial class RoleModel : ReactiveObject
{
    [Reactive]
    private int _zIndex;

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private Color _color = Colors.Transparent;
}
