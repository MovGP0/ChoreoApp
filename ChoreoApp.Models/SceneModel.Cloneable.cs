using DynamicData.Binding;

namespace ChoreoApp.Models;

public sealed partial class SceneModel : ICloneable<SceneModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public SceneModel Clone(CloneMode mode)
    {
        if (mode == CloneMode.Shallow)
        {
            return new SceneModel
            {
                SceneId = SceneId,
                Positions = Positions,
                Name = Name,
                Text = Text,
                FixedPositions = FixedPositions,
                Timestamp = Timestamp,
                VariationDepth = VariationDepth,
                Variations = Variations,
                CurrentVariation = CurrentVariation,
                Color = Color
            };
        }

        var sceneMap = new Dictionary<SceneModel, SceneModel>(ReferenceEqualityComparer.Instance);
        var dancerMap = new Dictionary<DancerModel, DancerModel>(ReferenceEqualityComparer.Instance);
        var roleMap = new Dictionary<RoleModel, RoleModel>(ReferenceEqualityComparer.Instance);
        return CloneInternal(this, sceneMap, dancerMap, roleMap);
    }

    internal static SceneModel CloneInternal(
        SceneModel source,
        Dictionary<SceneModel, SceneModel> sceneMap,
        Dictionary<DancerModel, DancerModel> dancerMap,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        if (sceneMap.TryGetValue(source, out var existing))
        {
            return existing;
        }

        var clone = new SceneModel
        {
            SceneId = source.SceneId,
            Name = source.Name,
            Text = source.Text,
            FixedPositions = source.FixedPositions,
            Timestamp = source.Timestamp,
            VariationDepth = source.VariationDepth,
            Color = source.Color
        };

        sceneMap[source] = clone;

        var positions = new ObservableCollectionExtended<PositionModel>();
        foreach (var position in source.Positions)
        {
            positions.Add(PositionModel.CloneInternal(position, dancerMap, roleMap));
        }

        clone.Positions = positions;

        var variationMap = new Dictionary<ObservableCollectionExtended<SceneModel>, ObservableCollectionExtended<SceneModel>>(
            ReferenceEqualityComparer.Instance);
        var variations = new ObservableCollectionExtended<ObservableCollectionExtended<SceneModel>>();
        foreach (var variation in source.Variations)
        {
            var clonedVariation = CloneVariation(variation, sceneMap, dancerMap, roleMap);
            variationMap[variation] = clonedVariation;
            variations.Add(clonedVariation);
        }

        clone.Variations = variations;

        if (variationMap.TryGetValue(source.CurrentVariation, out var currentVariation))
        {
            clone.CurrentVariation = currentVariation;
        }
        else
        {
            clone.CurrentVariation = CloneVariation(source.CurrentVariation, sceneMap, dancerMap, roleMap);
        }

        return clone;
    }

    private static ObservableCollectionExtended<SceneModel> CloneVariation(
        ObservableCollectionExtended<SceneModel> variation,
        Dictionary<SceneModel, SceneModel> sceneMap,
        Dictionary<DancerModel, DancerModel> dancerMap,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        var clone = new ObservableCollectionExtended<SceneModel>();
        foreach (var scene in variation)
        {
            clone.Add(CloneInternal(scene, sceneMap, dancerMap, roleMap));
        }

        return clone;
    }
}
