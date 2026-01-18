using ChoreoApp.Models;
using ChoreoApp.Scenes.Extensions;

namespace ChoreoApp.Scenes;

public sealed class SceneMapper
{
    public void Map(SceneViewModel source, SceneModel target)
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

        if (target.Variations is null)
        {
            target.Variations = new();
        }
        target.Variations.Clear();
        if (source.Variations is { } variations)
        {
            target.Variations.AddRange(variations.Select(v => CloneSceneList(v).AsObservableCollectionExtended()));
        }

        if (source.CurrentVariation is { } currentVariation)
        {
            target.CurrentVariation = CloneSceneList(currentVariation).AsObservableCollectionExtended();
        }

        target.Positions.Clear();
        foreach (var position in source.Positions)
        {
            target.Positions.Add(position);
        }
    }

    public void Map(SceneModel source, SceneViewModel target)
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

        if (source.Variations is {} variations)
        {
            target.Variations = variations
                .Select(v => (IList<SceneModel>)CloneSceneList(v).ToList())
                .ToList();
        }

        if (source.CurrentVariation is { } currentVariation)
        {
            target.CurrentVariation = CloneSceneList(currentVariation).ToList();
        }

        target.Positions.Clear();
        foreach (var position in source.Positions)
        {
            target.Positions.Add(position);
        }
    }

    private static IEnumerable<SceneModel> CloneSceneList(IEnumerable<SceneModel> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        foreach (var scene in source)
        {
            yield return CloneScene(scene);
        }
    }

    private static SceneModel CloneScene(SceneModel source)
    {
        var scene = new SceneModel
        {
            SceneId = source.SceneId,
            Name = source.Name,
            Text = source.Text,
            FixedPositions = source.FixedPositions,
            Timestamp = source.Timestamp,
            VariationDepth = source.VariationDepth,
            Color = source.Color
        };

        foreach (var position in source.Positions)
        {
            scene.Positions.Add(position);
        }

        if (source.Variations is { } variations)
        {
            scene.Variations = new();
            scene.Variations.AddRange(variations.Select(v => CloneSceneList(v).AsObservableCollectionExtended()));
        }

        if (source.CurrentVariation is { } currentVariation)
        {
            scene.CurrentVariation = CloneSceneList(currentVariation).AsObservableCollectionExtended();
        }

        return scene;
    }
}
