namespace ChoreoApp.Scenes.Events;

public enum CopyScenePositionsDecision
{
    CopyPositions,
    KeepPositions
}

public readonly struct CopyScenePositionsDecisionEvent
{
    public CopyScenePositionsDecisionEvent(CopyScenePositionsDecision decision)
    {
        Decision = decision;
    }

    public CopyScenePositionsDecision Decision { get; }
}
