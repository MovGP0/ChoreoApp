namespace MaterialDesignThemes.Maui;

public sealed class DialogClosingEventArgs : EventArgs
{
    public DialogClosingEventArgs(DialogSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        Session = session;
    }

    /// <summary>
    /// Cancel the close.
    /// </summary>
    public void Cancel() => IsCancelled = true;

    /// <summary>
    /// Indicates if the close has already been canceled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets the parameter originally provided to the close action.
    /// </summary>
    public object? Parameter => Session.CloseParameter;

    /// <summary>
    /// Allows interaction with the current dialog session.
    /// </summary>
    public DialogSession Session { get; }
}
