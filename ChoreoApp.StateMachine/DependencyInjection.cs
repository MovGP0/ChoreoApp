using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Transitions;
using ChoreoApp.StateMachine.Triggers;
using Microsoft.Extensions.DependencyInjection;

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
                FromState: typeof(ViewSceneState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewScenePanState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ViewSceneZoomState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsSelectionState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsDragState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsSelectionState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsDragState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsSelectionState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(MovePositionsDragState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
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
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionStartState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionEndState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationStartState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationEndState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionStartState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionEndState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragStartState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragEndState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragEndState),
                Trigger: typeof(MovePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new MovePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(RotateAroundCenterCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionStartState),
                Trigger: typeof(RotateAroundCenterCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionEndState),
                Trigger: typeof(RotateAroundCenterCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationStartState),
                Trigger: typeof(RotateAroundCenterCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationEndState),
                Trigger: typeof(RotateAroundCenterCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(RotateAroundCenterSelectionStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterSelectionStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionEndState),
                Trigger: typeof(RotateAroundCenterSelectionStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterSelectionStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationEndState),
                Trigger: typeof(RotateAroundCenterSelectionStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterSelectionStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionStartState),
                Trigger: typeof(RotateAroundCenterSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterSelectionEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(RotateAroundCenterSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterSelectionEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(RotateAroundCenterRotationStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterRotationStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionEndState),
                Trigger: typeof(RotateAroundCenterRotationStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterRotationStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationEndState),
                Trigger: typeof(RotateAroundCenterRotationStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterRotationStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationStartState),
                Trigger: typeof(RotateAroundCenterRotationCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterRotationEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionStartState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionEndState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationStartState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationEndState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionStartState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterSelectionEndState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationStartState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(RotateAroundCenterRotationEndState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(ScalePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionStartState),
                Trigger: typeof(ScalePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionEndState),
                Trigger: typeof(ScalePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragStartState),
                Trigger: typeof(ScalePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragEndState),
                Trigger: typeof(ScalePositionsCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(ScalePositionsSelectionStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsSelectionStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionStartState),
                Trigger: typeof(ScalePositionsSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsSelectionEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(ScalePositionsSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsSelectionEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(ScalePositionsDragStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsDragStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionEndState),
                Trigger: typeof(ScalePositionsDragStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsDragStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragStartState),
                Trigger: typeof(ScalePositionsDragCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsDragEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragEndState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragEndState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(ScaleAroundDancerCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(ScaleAroundDancerCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(ScaleAroundDancerCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(ScaleAroundDancerCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragEndState),
                Trigger: typeof(ScaleAroundDancerCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ViewSceneState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(ScaleAroundDancerSelectionStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerSelectionStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(ScaleAroundDancerSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerSelectionEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(ScaleAroundDancerSelectionCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerSelectionEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(ScaleAroundDancerDragStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerDragStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(ScaleAroundDancerDragStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerDragStartState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(ScaleAroundDancerDragCompletedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerDragEndState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionStartState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionEndState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragStartState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragEndState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionStartState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsSelectionEndState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragStartState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScalePositionsDragEndState),
                Trigger: typeof(ScaleAroundDancerStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScaleAroundDancerState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragEndState),
                Trigger: typeof(RotateAroundCenterStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new RotateAroundCenterState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionStartState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerSelectionEndState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragStartState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
            .AddSingleton<StateTransition>(_ => new(
                FromState: typeof(ScaleAroundDancerDragEndState),
                Trigger: typeof(ScalePositionsStartedTrigger),
                Preconditions: [],
                Apply: (_, _, _) => new ScalePositionsState()))
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
