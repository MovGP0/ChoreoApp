using ChoreoApp.Global;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Transitions;
using ChoreoApp.StateMachine.Triggers;

namespace ChoreoApp.StateMachine;

public sealed class ApplicationStateMachine
{
    public ApplicationStateMachine(
        ApplicationState initialState,
        IEnumerable<StateTransition> transitions)
    {
        ArgumentNullException.ThrowIfNull(initialState);
        ArgumentNullException.ThrowIfNull(transitions);

        State = initialState;
        _transitions = transitions.ToList();
    }

    private readonly List<StateTransition> _transitions;

    public ApplicationState State { get; private set; }

    public IReadOnlyList<StateTransition> Transitions => _transitions;

    public bool TryApply(GlobalStateModel globalState, ApplicationTrigger trigger)
    {
        ArgumentNullException.ThrowIfNull(globalState);
        ArgumentNullException.ThrowIfNull(trigger);

        var state = State;
        var stateType = state.GetType();
        var triggerType = trigger.GetType();

        foreach (var transition in _transitions)
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
