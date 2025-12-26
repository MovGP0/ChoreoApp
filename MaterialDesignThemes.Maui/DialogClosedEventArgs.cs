namespace MaterialDesignThemes.Maui;

public sealed class DialogClosedEventArgs : EventArgs
{
    public DialogClosedEventArgs(DialogSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        Session = session;
    }

    /// <summary>
    /// Gets the parameter originally provided to the close action.
    /// </summary>
    public object? Parameter => Session.CloseParameter;

    /// <summary>
    /// Allows interaction with the current dialog session.
    /// </summary>
    public DialogSession Session { get; }
}
