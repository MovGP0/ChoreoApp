# State Machine

## Purpose
The state machine models high-level interaction flow for the floor view. States are plain records and transitions are registered via dependency injection.

## Naming Convention
Use the pattern `<State><SubState>State` for all state records.
- Base states use only the main state name: `ViewSceneState`, `PlacePositionsState`.
- Sub-states append the interaction detail: `ViewScenePanState`, `ViewSceneZoomState`, `PlacePositionsPanState`, `PlacePositionsZoomState`.

Only the View-related states include `Scene` in the name; placement states do not.

## Usage
1) Define states as records in `ChoreoApp/StateMachine/States/`.
2) Define triggers as records in `ChoreoApp/StateMachine/Triggers/`.
3) Register transitions in `ChoreoApp/StateMachine/DependencyInjection.cs` using `StateTransition`.
   - Use `[]` for empty preconditions.
   - Keep transitions explicit even if they look similar; avoid clever merges so behavior stays obvious.
4) Emit triggers from behaviors (e.g., floor gesture handling) and call `ApplicationStateMachine.TryApply(trigger)`.

## Example (pattern)
```csharp
.AddSingleton<StateTransition>(_ => new(
    FromState: typeof(ViewSceneState),
    Trigger: typeof(PanStartedTrigger),
    Preconditions: [],
    Apply: (_, _, _) => new ViewScenePanState()))
```

## Notes
- The state machine is injected into behaviors that need to emit triggers.
- `ApplicationStateMachine.State` always holds the current state instance.
