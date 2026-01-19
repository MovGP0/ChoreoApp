using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;

namespace ChoreoApp.StateMachine.Transitions;

public sealed record StateTransition(
    Type FromState,
    Type Trigger,
    Func<IGlobalStateModel, ApplicationState, ApplicationTrigger, bool>[] Preconditions,
    Func<IGlobalStateModel, ApplicationState, ApplicationTrigger, ApplicationState> Apply)
{
    public bool CanApply(IGlobalStateModel globalState, ApplicationState state, ApplicationTrigger trigger)
        => Preconditions.Length == 0 || Preconditions.All(precondition => precondition(globalState, state, trigger));
}
