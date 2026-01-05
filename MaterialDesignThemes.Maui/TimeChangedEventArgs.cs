namespace MaterialDesignThemes.Maui;

public sealed class TimeChangedEventArgs : EventArgs
{
    public TimeChangedEventArgs(DateTime oldTime, DateTime newTime)
    {
        OldTime = oldTime;
        NewTime = newTime;
    }

    public DateTime OldTime { get; }
    public DateTime NewTime { get; }
}
