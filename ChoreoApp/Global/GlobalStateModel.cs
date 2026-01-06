using ChoreoApp.Scenes;
using ChoreoMasterMobile.Json;
using DynamicData.Binding;

namespace ChoreoApp.Global;

public sealed partial class GlobalStateModel : ReactiveObject
{
    [Reactive]
    private Choreography _choreography = new();

    [Reactive]
    private SvgDocument? _svgDocument;

    [Reactive]
    private string? _svgFilePath;

    [ReactiveCollection]
    private ObservableCollectionExtended<SceneViewModel> _scenes = [];

    [Reactive]
    private SceneViewModel? _selectedScene;

    [ReactiveCollection]
    private ObservableCollectionExtended<Position> _selectedPositions = [];

    [Reactive]
    private SelectionRectangle? _selectionRectangle;

    [Reactive]
    private InteractionMode _interactionMode = InteractionMode.View;

    [Reactive]
    private bool _isPlaceMode;
}
