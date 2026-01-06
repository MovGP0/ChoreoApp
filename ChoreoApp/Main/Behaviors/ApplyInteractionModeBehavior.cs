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
        if (mode == InteractionMode.Move)
        {
            stateMachine.TryApply(new MovePositionsStartedTrigger());
            return;
        }

        stateMachine.TryApply(new MovePositionsCompletedTrigger());
    }
}
