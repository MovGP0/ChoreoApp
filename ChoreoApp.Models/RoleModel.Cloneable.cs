namespace ChoreoApp.Models;

public sealed partial class RoleModel : ICloneable<RoleModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public RoleModel Clone(CloneMode mode)
    {
        return CloneInternal(this);
    }

    internal static RoleModel CloneInternal(RoleModel source)
    {
        return new RoleModel
        {
            ZIndex = source.ZIndex,
            Name = source.Name,
            Color = source.Color
        };
    }
}
