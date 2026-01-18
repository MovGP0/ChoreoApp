namespace ChoreoApp.Floor;

public sealed class FloorRenderGate : IFloorRenderGate
{
    private readonly TaskCompletionSource _rendered = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public bool IsRendered { get; private set; }

    public void MarkRendered()
    {
        if (IsRendered)
        {
            return;
        }

        IsRendered = true;
        _rendered.TrySetResult();
    }

    public async Task WaitForFirstRenderAsync(CancellationToken cancellationToken)
    {
        if (IsRendered)
        {
            return;
        }

        var delayTask = Task.Delay(Timeout.Infinite, cancellationToken);
        var completed = await Task.WhenAny(_rendered.Task, delayTask);
        if (completed == delayTask)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        await _rendered.Task;
    }
}
