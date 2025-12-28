using ChoreoApp.Scenes;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Transitions;
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

    [Reactive]
    private InteractionMode _interactionMode = InteractionMode.View;

    [Reactive]
    private bool _isPlaceMode;

    public ApplicationStateMachine StateMachine { get; } = new(
        initialState: new InitialApplicationState(),
        transitions: Array.Empty<StateTransition>());
}
