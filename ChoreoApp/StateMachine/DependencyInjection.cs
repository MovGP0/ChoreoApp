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
                Apply: (_, _, _) => new ViewScenePanState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(PanCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(ZoomStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneZoomState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(ZoomCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(ZoomStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneZoomState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(PanStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewScenePanState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsState),
                Trigger: typeof(MovePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsSelectionState),
                Trigger: typeof(MovePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsDragState),
                Trigger: typeof(MovePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsState),
                Trigger: typeof(MovePositionsSelectionStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsSelectionState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsSelectionState),
                Trigger: typeof(MovePositionsSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsState),
                Trigger: typeof(MovePositionsDragStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsDragState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsDragState),
                Trigger: typeof(MovePositionsDragCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(PlacePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(PlacePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(PlacePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsState),
                Trigger: typeof(PlacePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsPanState),
                Trigger: typeof(PlacePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsZoomState),
                Trigger: typeof(PlacePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsState),
                Trigger: typeof(PlacePositionsCanceledTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsPanState),
                Trigger: typeof(PlacePositionsCanceledTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsZoomState),
                Trigger: typeof(PlacePositionsCanceledTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsState),
                Trigger: typeof(PanStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsPanState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsPanState),
                Trigger: typeof(PanCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsState),
                Trigger: typeof(ZoomStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsZoomState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsZoomState),
                Trigger: typeof(ZoomCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsPanState),
                Trigger: typeof(ZoomStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsZoomState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(PlacePositionsZoomState),
                Trigger: typeof(PanStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new PlacePositionsPanState()));
    }
}
