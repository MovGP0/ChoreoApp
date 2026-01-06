namespace ChoreoApp.Floor;

public interface IFloorRenderGate
{
    bool IsRendered { get; }
    void MarkRendered();
    Task WaitForFirstRenderAsync(CancellationToken cancellationToken);
}
