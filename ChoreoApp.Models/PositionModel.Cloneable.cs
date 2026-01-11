namespace ChoreoApp.Models;

public sealed partial class PositionModel : ICloneable<PositionModel>
{
    public object Clone() => Clone(CloneMode.Deep);

    public PositionModel Clone(CloneMode mode)
    {
        if (mode == CloneMode.Shallow)
        {
            return new PositionModel
            {
                Dancer = Dancer,
                Orientation = Orientation,
                X = X,
                Y = Y,
                Curve1X = Curve1X,
                Curve1Y = Curve1Y,
                Curve2X = Curve2X,
                Curve2Y = Curve2Y,
                Movement1X = Movement1X,
                Movement1Y = Movement1Y,
                Movement2X = Movement2X,
                Movement2Y = Movement2Y
            };
        }

        var dancerMap = new Dictionary<DancerModel, DancerModel>(ReferenceEqualityComparer.Instance);
        var roleMap = new Dictionary<RoleModel, RoleModel>(ReferenceEqualityComparer.Instance);
        return CloneInternal(this, dancerMap, roleMap);
    }

    internal static PositionModel CloneInternal(
        PositionModel source,
        Dictionary<DancerModel, DancerModel> dancerMap,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        return new PositionModel
        {
            Dancer = CloneDancer(source.Dancer, dancerMap, roleMap),
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
    }

    private static DancerModel? CloneDancer(
        DancerModel? dancer,
        Dictionary<DancerModel, DancerModel> dancerMap,
        Dictionary<RoleModel, RoleModel> roleMap)
    {
        if (dancer is null)
        {
            return null;
        }

        if (dancerMap.TryGetValue(dancer, out var existing))
        {
            return existing;
        }

        var clone = DancerModel.CloneInternal(dancer, roleMap);
        dancerMap[dancer] = clone;
        return clone;
    }
}
