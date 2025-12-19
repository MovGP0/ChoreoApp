namespace ChoreoApp.Scenes;

public sealed class SceneMapper
{
    public void Map(SceneViewModel source, ChoreoMasterMobile.Json.Scene target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        target.SceneId = source.SceneId;
        target.Name = source.Name;
        target.Timestamp = source.Timestamp;
        target.Color = source.Color;
        target.Text = source.Text;
        target.FixedPositions = source.FixedPositions;
        target.VariationDepth = source.VariationDepth;
        target.Variations = CloneSceneListList(source.Variations);
        target.CurrentVariation = CloneSceneList(source.CurrentVariation);

        target.Positions ??= new List<ChoreoMasterMobile.Json.Position>();
        target.Positions.Clear();

        foreach (var position in source.Positions)
        {
            target.Positions.Add(position);
        }
    }

    public void Map(ChoreoMasterMobile.Json.Scene source, SceneViewModel target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        target.SceneId = source.SceneId;
        target.Name = source.Name;
        target.Timestamp = source.Timestamp;
        target.Color = source.Color;
        target.Text = source.Text ?? string.Empty;
        target.FixedPositions = source.FixedPositions;
        target.VariationDepth = source.VariationDepth;
        target.Variations = CloneSceneListList(source.Variations);
        target.CurrentVariation = CloneSceneList(source.CurrentVariation);

        target.Positions.Clear();
        if (source.Positions is not null)
        {
            foreach (var position in source.Positions)
            {
                target.Positions.Add(position);
            }
        }
    }

    private static IList<IList<ChoreoMasterMobile.Json.Scene>>? CloneSceneListList(
        IList<IList<ChoreoMasterMobile.Json.Scene>>? source)
    {
        if (source is null)
        {
            return null;
        }

        var result = new List<IList<ChoreoMasterMobile.Json.Scene>>(source.Count);
        foreach (var list in source)
        {
            result.Add(CloneSceneList(list) ?? new List<ChoreoMasterMobile.Json.Scene>());
        }

        return result;
    }

    private static IList<ChoreoMasterMobile.Json.Scene>? CloneSceneList(
        IList<ChoreoMasterMobile.Json.Scene>? source)
    {
        if (source is null)
        {
            return null;
        }

        var result = new List<ChoreoMasterMobile.Json.Scene>(source.Count);
        foreach (var scene in source)
        {
            result.Add(CloneScene(scene));
        }

        return result;
    }

    private static ChoreoMasterMobile.Json.Scene CloneScene(ChoreoMasterMobile.Json.Scene source)
    {
        var scene = new ChoreoMasterMobile.Json.Scene
        {
            SceneId = source.SceneId,
            Name = source.Name,
            Text = source.Text,
            FixedPositions = source.FixedPositions,
            Timestamp = source.Timestamp,
            VariationDepth = source.VariationDepth,
            Color = source.Color
        };

        if (source.Positions is not null)
        {
            scene.Positions = new List<ChoreoMasterMobile.Json.Position>(source.Positions);
        }

        scene.Variations = CloneSceneListList(source.Variations);
        scene.CurrentVariation = CloneSceneList(source.CurrentVariation);

        return scene;
    }
}
