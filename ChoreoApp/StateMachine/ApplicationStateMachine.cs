using ChoreoApp.Global;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Transitions;
using ChoreoApp.StateMachine.Triggers;

namespace ChoreoApp.StateMachine;

public sealed class ApplicationStateMachine(
    GlobalStateModel globalState,
    IEnumerable<StateTransition> transitions)
{
    public ApplicationState State { get; private set; } = new InitialApplicationState();

    public bool TryApply(ApplicationTrigger trigger)
    {
        ArgumentNullException.ThrowIfNull(globalState);
        ArgumentNullException.ThrowIfNull(trigger);

        var state = State;
        var stateType = state.GetType();
        var triggerType = trigger.GetType();

        foreach (var transition in transitions)
        {
            if (!transition.FromState.IsAssignableFrom(stateType)
                || !transition.Trigger.IsAssignableFrom(triggerType))
            {
                continue;
            }

            if (!transition.CanApply(globalState, state, trigger))
            {
                continue;
            }

            State = transition.Apply(globalState, state, trigger);
            return true;
        }

        return false;
    }
}
