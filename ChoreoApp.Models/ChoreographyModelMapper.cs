using ChoreoMasterMobile.Json;
using DynamicData.Binding;

namespace ChoreoApp.Models;

public sealed partial class ChoreographyModelMapper
{
    public ChoreographyModel Map(Choreography source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var model = new ChoreographyModel
        {
            Comment = source.Comment,
            Name = source.Name,
            Subtitle = source.Subtitle,
            Date = source.Date,
            Variation = source.Variation,
            Author = source.Author,
            Description = source.Description,
            LastSaveDate = source.LastSaveDate,
            Settings = MapSettings(source.Settings),
            Floor = MapFloor(source.Floor)
        };

        var roleMap = new Dictionary<Role, RoleModel>(ReferenceEqualityComparer.Instance);
        foreach (var role in source.Roles)
        {
            var roleModel = MapRole(role);
            roleMap[role] = roleModel;
            model.Roles.Add(roleModel);
        }

        var dancerMap = new Dictionary<Dancer, DancerModel>(ReferenceEqualityComparer.Instance);
        foreach (var dancer in source.Dancers)
        {
            var roleModel = roleMap.TryGetValue(dancer.Role, out var mappedRole)
                ? mappedRole
                : MapRole(dancer.Role);

            var dancerModel = MapDancer(dancer, roleModel);
            dancerMap[dancer] = dancerModel;
            model.Dancers.Add(dancerModel);
        }

        foreach (var scene in source.Scenes)
        {
            model.Scenes.Add(MapScene(scene, dancerMap));
        }

        return model;
    }

    public Choreography Map(ChoreographyModel source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var target = new Choreography
        {
            Comment = source.Comment,
            Name = source.Name,
            Subtitle = source.Subtitle,
            Date = source.Date,
            Variation = source.Variation,
            Author = source.Author,
            Description = source.Description,
            LastSaveDate = source.LastSaveDate,
            Settings = MapSettings(source.Settings),
            Floor = MapFloor(source.Floor)
        };

        var roleMap = new Dictionary<RoleModel, Role>(ReferenceEqualityComparer.Instance);
        foreach (var role in source.Roles)
        {
            var roleModel = MapRole(role);
            roleMap[role] = roleModel;
            target.Roles.Add(roleModel);
        }

        var dancerMap = new Dictionary<DancerModel, Dancer>(ReferenceEqualityComparer.Instance);
        foreach (var dancer in source.Dancers)
        {
            var role = roleMap.TryGetValue(dancer.Role, out var mappedRole)
                ? mappedRole
                : MapRole(dancer.Role);

            var dancerModel = MapDancer(dancer, role);
            dancerMap[dancer] = dancerModel;
            target.Dancers.Add(dancerModel);
        }

        foreach (var scene in source.Scenes)
        {
            target.Scenes.Add(MapScene(scene, dancerMap));
        }

        return target;
    }

    private static SettingsModel MapSettings(ChoreoMasterMobile.Json.Settings source)
    {
        return new SettingsModel
        {
            AnimationMilliseconds = source.AnimationMilliseconds,
            FrontPosition = source.FrontPosition,
            DancerPosition = source.DancerPosition,
            Resolution = source.Resolution,
            Transparency = source.Transparency,
            PositionsAtSide = source.PositionsAtSide,
            GridLines = source.GridLines,
            FloorColor = source.FloorColor,
            DancerSize = source.DancerSize,
            ShowTimestamps = source.ShowTimestamps,
            MusicPathAbsolute = source.MusicPathAbsolute,
            MusicPathRelative = source.MusicPathRelative
        };
    }

    private static ChoreoMasterMobile.Json.Settings MapSettings(SettingsModel source)
    {
        return new ChoreoMasterMobile.Json.Settings
        {
            AnimationMilliseconds = source.AnimationMilliseconds,
            FrontPosition = source.FrontPosition,
            DancerPosition = source.DancerPosition,
            Resolution = source.Resolution,
            Transparency = source.Transparency,
            PositionsAtSide = source.PositionsAtSide,
            GridLines = source.GridLines,
            FloorColor = source.FloorColor,
            DancerSize = source.DancerSize,
            ShowTimestamps = source.ShowTimestamps,
            MusicPathAbsolute = source.MusicPathAbsolute,
            MusicPathRelative = source.MusicPathRelative
        };
    }

    private static FloorModel MapFloor(ChoreoMasterMobile.Json.Floor source)
    {
        return new FloorModel
        {
            SizeFront = source.SizeFront,
            SizeBack = source.SizeBack,
            SizeLeft = source.SizeLeft,
            SizeRight = source.SizeRight
        };
    }

    private static ChoreoMasterMobile.Json.Floor MapFloor(FloorModel source)
    {
        return new ChoreoMasterMobile.Json.Floor
        {
            SizeFront = source.SizeFront,
            SizeBack = source.SizeBack,
            SizeLeft = source.SizeLeft,
            SizeRight = source.SizeRight
        };
    }

    private static RoleModel MapRole(Role source)
    {
        return new RoleModel
        {
            ZIndex = source.ZIndex,
            Name = source.Name,
            Color = source.Color
        };
    }

    private static Role MapRole(RoleModel source)
    {
        return new Role
        {
            ZIndex = source.ZIndex,
            Name = source.Name,
            Color = source.Color
        };
    }

    private static DancerModel MapDancer(Dancer source, RoleModel role)
    {
        return new DancerModel
        {
            DancerId = source.DancerId,
            Role = role,
            Name = source.Name,
            Shortcut = source.Shortcut,
            Color = source.Color,
            Icon = source.Icon
        };
    }

    private static Dancer MapDancer(DancerModel source, Role role)
    {
        return new Dancer
        {
            DancerId = source.DancerId,
            Role = role,
            Name = source.Name,
            Shortcut = source.Shortcut,
            Color = source.Color,
            Icon = source.Icon
        };
    }

    private static SceneModel MapScene(Scene source, IReadOnlyDictionary<Dancer, DancerModel> dancerMap)
    {
        var model = new SceneModel
        {
            SceneId = source.SceneId,
            Name = source.Name,
            Text = source.Text,
            FixedPositions = source.FixedPositions,
            Timestamp = source.Timestamp,
            VariationDepth = source.VariationDepth,
            Color = source.Color,
            Variations = MapSceneVariations(source.Variations, dancerMap),
            CurrentVariation = MapSceneList(source.CurrentVariation, dancerMap)
        };

        if (source.Positions is not null)
        {
            foreach (var position in source.Positions)
            {
                model.Positions.Add(MapPosition(position, dancerMap));
            }
        }

        return model;
    }

    private static Scene MapScene(SceneModel source, IReadOnlyDictionary<DancerModel, Dancer> dancerMap)
    {
        var scene = new Scene
        {
            SceneId = source.SceneId,
            Name = source.Name,
            Text = source.Text,
            FixedPositions = source.FixedPositions,
            Timestamp = source.Timestamp,
            VariationDepth = source.VariationDepth,
            Color = source.Color,
            Variations = MapSceneVariations(source.Variations, dancerMap),
            CurrentVariation = MapSceneList(source.CurrentVariation, dancerMap)
        };

        if (source.Positions.Count > 0)
        {
            var positions = new List<Position>(source.Positions.Count);
            foreach (var position in source.Positions)
            {
                positions.Add(MapPosition(position, dancerMap));
            }

            scene.Positions = positions;
        }

        return scene;
    }

    private static PositionModel MapPosition(Position source, IReadOnlyDictionary<Dancer, DancerModel> dancerMap)
    {
        var model = new PositionModel
        {
            Orientation = source.Orientation,
            X = source.X,
            Y = source.Y,
            Curve1X = source.Curve1X,
            Curve1Y = source.Curve1Y,
            Curve2X = source.Curve2X,
            Curve2Y = source.Curve2Y,
            Movement1X = source.Movement1X,
            Movement1Y = source.Movement1Y,
            Movement2X = source.Movement2X,
            Movement2Y = source.Movement2Y
        };

        if (source.Dancer is not null)
        {
            if (dancerMap.TryGetValue(source.Dancer, out var dancer))
            {
                model.Dancer = dancer;
            }
            else
            {
                model.Dancer = MapDancer(source.Dancer, MapRole(source.Dancer.Role));
            }
        }

        return model;
    }

    private static Position MapPosition(PositionModel source, IReadOnlyDictionary<DancerModel, Dancer> dancerMap)
    {
        var position = new Position
        {
            Orientation = source.Orientation,
            X = source.X,
            Y = source.Y,
            Curve1X = source.Curve1X,
            Curve1Y = source.Curve1Y,
            Curve2X = source.Curve2X,
            Curve2Y = source.Curve2Y,
            Movement1X = source.Movement1X,
            Movement1Y = source.Movement1Y,
            Movement2X = source.Movement2X,
            Movement2Y = source.Movement2Y
        };

        if (source.Dancer is not null)
        {
            if (dancerMap.TryGetValue(source.Dancer, out var dancer))
            {
                position.Dancer = dancer;
            }
            else
            {
                position.Dancer = MapDancer(source.Dancer, MapRole(source.Dancer.Role));
            }
        }

        return position;
    }

    private static ObservableCollectionExtended<ObservableCollectionExtended<SceneModel>> MapSceneVariations(
        IList<IList<Scene>>? variations,
        IReadOnlyDictionary<Dancer, DancerModel> dancerMap)
    {
        if (variations is null)
        {
            return new();
        }

        var result = new ObservableCollectionExtended<ObservableCollectionExtended<SceneModel>>();
        foreach (var list in variations)
        {
            var mappedList = new ObservableCollectionExtended<SceneModel>();
            foreach (var scene in list)
            {
                mappedList.Add(MapScene(scene, dancerMap));
            }

            result.Add(mappedList);
        }

        return result;
    }

    private static IList<IList<Scene>>? MapSceneVariations(
        ObservableCollectionExtended<ObservableCollectionExtended<SceneModel>>? variations,
        IReadOnlyDictionary<DancerModel, Dancer> dancerMap)
    {
        if (variations is null)
        {
            return null;
        }

        var result = new List<IList<Scene>>(variations.Count);
        foreach (var list in variations)
        {
            var mappedList = new List<Scene>(list.Count);
            foreach (var scene in list)
            {
                mappedList.Add(MapScene(scene, dancerMap));
            }

            result.Add(mappedList);
        }

        return result;
    }

    private static ObservableCollectionExtended<SceneModel> MapSceneList(
        IList<Scene>? scenes,
        IReadOnlyDictionary<Dancer, DancerModel> dancerMap)
    {
        if (scenes is null)
        {
            return new();
        }

        var result = new ObservableCollectionExtended<SceneModel>();
        foreach (var scene in scenes)
        {
            result.Add(MapScene(scene, dancerMap));
        }

        return result;
    }

    private static IList<Scene>? MapSceneList(
        ObservableCollectionExtended<SceneModel>? scenes,
        IReadOnlyDictionary<DancerModel, Dancer> dancerMap)
    {
        if (scenes is null)
        {
            return null;
        }

        var result = new List<Scene>(scenes.Count);
        foreach (var scene in scenes)
        {
            result.Add(MapScene(scene, dancerMap));
        }

        return result;
    }
}
