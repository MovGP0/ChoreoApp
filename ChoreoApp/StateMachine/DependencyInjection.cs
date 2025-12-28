using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Transitions;
using ChoreoApp.StateMachine.Triggers;

namespace ChoreoApp.StateMachine;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationStateMachine(this IServiceCollection services)
    {
        return services
            .AddSingleton<ApplicationStateMachine>()
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(PanStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PanViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PanViewSceneState),
                Trigger: typeof(PanCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(ZoomStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ZoomViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ZoomViewSceneState),
                Trigger: typeof(ZoomCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PanViewSceneState),
                Trigger: typeof(ZoomStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ZoomViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ZoomViewSceneState),
                Trigger: typeof(PanStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PanViewSceneState()));
    }
}
