using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.Triggers;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Main.Behaviors;

public sealed class ApplyInteractionModeBehavior(
    GlobalStateModel globalState,
    ApplicationStateMachine stateMachine,
    ILogger<MainViewModel> logger)
    : IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ApplyInteractionModeBehavior), nameof(MainViewModel));
        globalState
            .WhenAnyValue(state => state.InteractionMode)
            .Subscribe(ApplyMode)
            .DisposeWith(disposables);
    }

    private void ApplyMode(InteractionMode mode)
    {
        switch (mode)
        {
            case InteractionMode.Move:
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                stateMachine.TryApply(new ScalePositionsCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerCompletedTrigger());
                stateMachine.TryApply(new MovePositionsStartedTrigger());
                break;
            case InteractionMode.RotateAroundCenter when globalState.SelectedPositions.Count == 0:
                stateMachine.TryApply(new ScalePositionsCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerCompletedTrigger());
                stateMachine.TryApply(new RotateAroundCenterStartedTrigger());
                break;
            case InteractionMode.RotateAroundCenter:
                stateMachine.TryApply(new ScalePositionsCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerCompletedTrigger());
                stateMachine.TryApply(new RotateAroundCenterStartedTrigger());
                stateMachine.TryApply(new RotateAroundCenterSelectionCompletedTrigger());
                break;
            case InteractionMode.RotateAroundDancer when globalState.SelectedPositions.Count == 0:
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                stateMachine.TryApply(new ScalePositionsCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerStartedTrigger());
                break;
            case InteractionMode.RotateAroundDancer:
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                stateMachine.TryApply(new ScalePositionsCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerStartedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerSelectionCompletedTrigger());
                break;
            case InteractionMode.Scale when globalState.SelectedPositions.Count == 0:
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerCompletedTrigger());
                stateMachine.TryApply(new ScalePositionsStartedTrigger());
                break;
            case InteractionMode.Scale:
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerCompletedTrigger());
                stateMachine.TryApply(new ScalePositionsStartedTrigger());
                stateMachine.TryApply(new ScalePositionsSelectionCompletedTrigger());
                break;
            default:
                stateMachine.TryApply(new MovePositionsCompletedTrigger());
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                stateMachine.TryApply(new ScalePositionsCompletedTrigger());
                stateMachine.TryApply(new ScaleAroundDancerCompletedTrigger());
                break;
        }
    }
}
