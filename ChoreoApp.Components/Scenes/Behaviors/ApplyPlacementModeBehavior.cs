using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.Triggers;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class ApplyPlacementModeBehavior(
    Global.GlobalStateModel globalState,
    ApplicationStateMachine stateMachine,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber,
    ILogger<ScenesPaneViewModel> logger)
    : IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ApplyPlacementModeBehavior), nameof(ScenesPaneViewModel));
        selectedSceneChangedSubscriber
            .Subscribe(evt => HandleSceneSelected(evt.SelectedScene))
            .DisposeWith(disposables);
    }

    private void HandleSceneSelected(SceneViewModel? scene)
    {
        if (globalState.Choreography is null)
        {
            globalState.IsPlaceMode = false;
            stateMachine.TryApply(new PlacePositionsCompletedTrigger());
            return;
        }

        if (scene is null)
        {
            globalState.IsPlaceMode = false;
            stateMachine.TryApply(new PlacePositionsCompletedTrigger());
            return;
        }

        int dancerCount = globalState.Choreography.Dancers.Count;
        int positionCount = scene.Positions.Count;
        var shouldPlace = dancerCount > 0 && positionCount < dancerCount;

        globalState.IsPlaceMode = shouldPlace;
        stateMachine.TryApply(shouldPlace
            ? new PlacePositionsStartedTrigger()
            : new PlacePositionsCompletedTrigger());

        if (shouldPlace)
        {
            ClearAssignedDancers(scene);
        }
    }

    private static void ClearAssignedDancers(SceneViewModel scene)
    {
        foreach (var position in scene.Positions)
        {
            position.Dancer = null;
        }
    }
}
