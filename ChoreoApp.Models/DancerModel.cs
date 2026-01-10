using ChoreoMasterMobile.Json;

namespace ChoreoApp.Models;

public sealed partial class DancerModel : ReactiveObject
{
    [Reactive]
    private DancerId _dancerId = DancerId.Empty;

    [Reactive]
    private RoleModel _role = null!;

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private string _shortcut = string.Empty;

    [Reactive]
    private Color _color = Colors.Transparent;

    [Reactive]
    private string? _icon;
}
