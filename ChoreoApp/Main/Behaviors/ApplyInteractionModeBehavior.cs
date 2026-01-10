using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.Triggers;

namespace ChoreoApp.Main.Behaviors;

public sealed class ApplyInteractionModeBehavior(
    GlobalStateModel globalState,
    ApplicationStateMachine stateMachine)
    : IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
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
                stateMachine.TryApply(new MovePositionsStartedTrigger());
                break;
            case InteractionMode.RotateAroundCenter when globalState.SelectedPositions.Count == 0:
                stateMachine.TryApply(new RotateAroundCenterStartedTrigger());
                break;
            case InteractionMode.RotateAroundCenter:
                stateMachine.TryApply(new RotateAroundCenterSelectionCompletedTrigger());
                break;
            default:
                stateMachine.TryApply(new MovePositionsCompletedTrigger());
                stateMachine.TryApply(new RotateAroundCenterCompletedTrigger());
                break;
        }
    }
}
