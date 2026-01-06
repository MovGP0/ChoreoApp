using ChoreoMasterMobile.Json;
using DynamicData.Binding;

namespace ChoreoApp.Models;

public sealed partial class SceneModel : ReactiveObject
{
    [Reactive]
    private SceneId _sceneId;

    [ReactiveCollection]
    private ObservableCollectionExtended<PositionModel> _positions = [];

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private string? _text;

    [Reactive]
    private bool _fixedPositions;

    [Reactive]
    private TimeSpan? _timestamp;

    [Reactive]
    private int _variationDepth;

    [Reactive]
    private ObservableCollectionExtended<ObservableCollectionExtended<SceneModel>> _variations = new();

    [Reactive]
    private ObservableCollectionExtended<SceneModel> _currentVariation = new();

    [Reactive]
    private Color _color = Colors.Transparent;
}
