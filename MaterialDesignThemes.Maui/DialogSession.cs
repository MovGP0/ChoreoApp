namespace MaterialDesignThemes.Maui;

/// <summary>
/// Allows an open dialog to be managed. Use is only permitted during a single display operation.
/// </summary>
public sealed class DialogSession
{
    private readonly DialogHost _owner;

    internal DialogSession(DialogHost owner)
    {
        ArgumentNullException.ThrowIfNull(owner);
        _owner = owner;
    }

    /// <summary>
    /// Indicates if the dialog session has ended. Once ended, no further method calls are permitted.
    /// </summary>
    public bool IsEnded { get; internal set; }

    internal object? CloseParameter { get; set; }

    /// <summary>
    /// Gets the content currently displayed in the dialog.
    /// </summary>
    public object? Content => _owner.DialogContent;

    /// <summary>
    /// Update the current content in the dialog.
    /// </summary>
    public void UpdateContent(object? content)
    {
        _owner.DialogContent = content;
        _owner.Dispatcher.Dispatch(() => _owner.Focus());
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public void Close()
    {
        if (IsEnded)
        {
            throw new InvalidOperationException("Dialog session has ended.");
        }

        _owner.InternalClose(null);
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    /// <param name="parameter">Result parameter which will be returned on close.</param>
    public void Close(object? parameter)
    {
        if (IsEnded)
        {
            throw new InvalidOperationException("Dialog session has ended.");
        }

        _owner.InternalClose(parameter);
    }
}
