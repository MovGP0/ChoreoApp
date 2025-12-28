using ChoreoApp.Global;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;

namespace ChoreoApp.StateMachine.Transitions;

public sealed record StateTransition(
    Type FromState,
    Type Trigger,
    Func<GlobalStateModel, ApplicationState, ApplicationTrigger, bool>[] Preconditions,
    Func<GlobalStateModel, ApplicationState, ApplicationTrigger, ApplicationState> Apply)
{
    public bool CanApply(GlobalStateModel globalState, ApplicationState state, ApplicationTrigger trigger)
        => Preconditions.Length == 0 || Preconditions.All(precondition => precondition(globalState, state, trigger));
}
