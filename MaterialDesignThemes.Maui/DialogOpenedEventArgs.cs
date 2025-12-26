namespace MaterialDesignThemes.Maui;

public sealed class DialogOpenedEventArgs : EventArgs
{
    public DialogOpenedEventArgs(DialogSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        Session = session;
    }

    /// <summary>
    /// Allows interaction with the current dialog session.
    /// </summary>
    public DialogSession Session { get; }
}
