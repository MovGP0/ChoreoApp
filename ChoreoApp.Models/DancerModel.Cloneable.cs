namespace ChoreoApp.Models;

public sealed partial class DancerModel : ICloneable<DancerModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public DancerModel Clone(CloneMode mode)
    {
        if (mode == CloneMode.Shallow)
        {
            return new DancerModel
            {
                DancerId = DancerId,
                Role = Role,
                Name = Name,
                Shortcut = Shortcut,
                Color = Color,
                Icon = Icon
            };
        }

        var roleMap = new Dictionary<RoleModel, RoleModel>(ReferenceEqualityComparer.Instance);
        return CloneInternal(this, roleMap);
    }

    internal static DancerModel CloneInternal(DancerModel source, Dictionary<RoleModel, RoleModel> roleMap)
    {
        var roleClone = CloneRole(source.Role, roleMap);
        return new DancerModel
        {
            DancerId = source.DancerId,
            Role = roleClone,
            Name = source.Name,
            Shortcut = source.Shortcut,
            Color = source.Color,
            Icon = source.Icon
        };
    }

    private static RoleModel CloneRole(RoleModel role, Dictionary<RoleModel, RoleModel> roleMap)
    {
        if (roleMap.TryGetValue(role, out var existing))
        {
            return existing;
        }

        var clone = RoleModel.CloneInternal(role);
        roleMap[role] = clone;
        return clone;
    }
}
