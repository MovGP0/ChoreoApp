using DynamicData.Binding;

namespace ChoreoApp.Models;

public sealed partial class ChoreographyModel : ICloneable<ChoreographyModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public ChoreographyModel Clone(CloneMode mode)
    {
        if (mode == CloneMode.Shallow)
        {
            return new ChoreographyModel
            {
                Comment = Comment,
                Settings = Settings,
                Floor = Floor,
                Roles = Roles,
                Dancers = Dancers,
                Scenes = Scenes,
                Name = Name,
                Subtitle = Subtitle,
                Date = Date,
                Variation = Variation,
                Author = Author,
                Description = Description,
                LastSaveDate = LastSaveDate
            };
        }

        var roleMap = new Dictionary<RoleModel, RoleModel>(ReferenceEqualityComparer.Instance);
        var dancerMap = new Dictionary<DancerModel, DancerModel>(ReferenceEqualityComparer.Instance);
        var sceneMap = new Dictionary<SceneModel, SceneModel>(ReferenceEqualityComparer.Instance);

        var clone = new ChoreographyModel
        {
            Comment = Comment,
            Settings = (SettingsModel)Settings.Clone(),
            Floor = (FloorModel)Floor.Clone(),
            Name = Name,
            Subtitle = Subtitle,
            Date = Date,
            Variation = Variation,
            Author = Author,
            Description = Description,
            LastSaveDate = LastSaveDate
        };

        clone.Roles = CloneRoles(Roles, roleMap);
        clone.Dancers = CloneDancers(Dancers, dancerMap, roleMap);
        clone.Scenes = CloneScenes(Scenes, sceneMap, dancerMap, roleMap);

        return clone;
    }

    private static ObservableCollectionExtended<RoleModel> CloneRoles(
        IEnumerable<RoleModel> roles,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        var result = new ObservableCollectionExtended<RoleModel>();
        foreach (var role in roles)
        {
            if (!roleMap.TryGetValue(role, out var roleClone))
            {
                roleClone = RoleModel.CloneInternal(role);
                roleMap[role] = roleClone;
            }

            result.Add(roleClone);
        }

        return result;
    }

    private static ObservableCollectionExtended<DancerModel> CloneDancers(
        IEnumerable<DancerModel> dancers,
        Dictionary<DancerModel, DancerModel> dancerMap,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        var result = new ObservableCollectionExtended<DancerModel>();
        foreach (var dancer in dancers)
        {
            if (!dancerMap.TryGetValue(dancer, out var dancerClone))
            {
                dancerClone = DancerModel.CloneInternal(dancer, roleMap);
                dancerMap[dancer] = dancerClone;
            }

            result.Add(dancerClone);
        }

        return result;
    }

    private static ObservableCollectionExtended<SceneModel> CloneScenes(
        IEnumerable<SceneModel> scenes,
        Dictionary<SceneModel, SceneModel> sceneMap,
        Dictionary<DancerModel, DancerModel> dancerMap,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        var result = new ObservableCollectionExtended<SceneModel>();
        foreach (var scene in scenes)
        {
            result.Add(SceneModel.CloneInternal(scene, sceneMap, dancerMap, roleMap));
        }

        return result;
    }
}
