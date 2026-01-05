namespace MaterialDesignThemes.Maui;

/// <summary>
/// Helper extensions for showing dialogs.
/// </summary>
public static class DialogHostEx
{
    public static Task<object?> ShowDialog(this Window window, object content)
        => GetFirstDialogHost(window).ShowInternal(content, null, null, null);

    public static Task<object?> ShowDialog(this Window window, object content, DialogOpenedEventHandler openedEventHandler)
        => GetFirstDialogHost(window).ShowInternal(content, openedEventHandler, null, null);

    public static Task<object?> ShowDialog(this Window window, object content, object? dialogIdentifier)
        => GetFirstDialogHost(window, dialogIdentifier).ShowInternal(content, null, null, null);

    public static Task<object?> ShowDialog(this Window window, object content, DialogClosingEventHandler closingEventHandler)
        => GetFirstDialogHost(window).ShowInternal(content, null, closingEventHandler, null);

    public static Task<object?> ShowDialog(this Window window, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        => GetFirstDialogHost(window).ShowInternal(content, openedEventHandler, closingEventHandler, null);

    public static Task<object?> ShowDialog(this Window window, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        => GetFirstDialogHost(window).ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);

    public static Task<object?> ShowDialog(this Element childElement, object content)
        => GetOwningDialogHost(childElement).ShowInternal(content, null, null, null);

    public static Task<object?> ShowDialog(this Element childElement, object content, object dialogIdentifier)
        => GetOwningDialogHost(childElement, dialogIdentifier).ShowInternal(content, null, null, null);

    public static Task<object?> ShowDialog(this Element childElement, object content, DialogOpenedEventHandler openedEventHandler)
        => GetOwningDialogHost(childElement).ShowInternal(content, openedEventHandler, null, null);

    public static Task<object?> ShowDialog(this Element childElement, object content, DialogClosingEventHandler closingEventHandler)
        => GetOwningDialogHost(childElement).ShowInternal(content, null, closingEventHandler, null);

    public static Task<object?> ShowDialog(this Element childElement, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        => GetOwningDialogHost(childElement).ShowInternal(content, openedEventHandler, closingEventHandler, null);

    public static Task<object?> ShowDialog(this Element childElement, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        => GetOwningDialogHost(childElement).ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);

    private static DialogHost GetFirstDialogHost(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (window.Page is null)
        {
            throw new InvalidOperationException("Unable to find a DialogHost in visual tree");
        }

        DialogHost? dialogHost = window.Page.VisualDepthFirstTraversal().OfType<DialogHost>().FirstOrDefault();
        if (dialogHost is null)
        {
            throw new InvalidOperationException("Unable to find a DialogHost in visual tree");
        }

        return dialogHost;
    }

    private static DialogHost GetFirstDialogHost(Window window, object? dialogIdentifier)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (window.Page is null)
        {
            throw new InvalidOperationException($"Unable to find a DialogHost with identifier '{dialogIdentifier}' in visual tree");
        }

        DialogHost? dialogHost = window.Page.VisualDepthFirstTraversal()
            .OfType<DialogHost>()
            .FirstOrDefault(x => x.Identifier is not null && x.Identifier.Equals(dialogIdentifier));

        if (dialogHost is null)
        {
            throw new InvalidOperationException($"Unable to find a DialogHost with identifier '{dialogIdentifier}' in visual tree");
        }

        return dialogHost;
    }

    private static DialogHost GetOwningDialogHost(Element childElement)
    {
        ArgumentNullException.ThrowIfNull(childElement);

        DialogHost? dialogHost = childElement.GetVisualAncestry().OfType<DialogHost>().FirstOrDefault();
        if (dialogHost is null)
        {
            throw new InvalidOperationException("Unable to find a DialogHost in visual tree ancestry");
        }

        return dialogHost;
    }

    private static DialogHost GetOwningDialogHost(Element childElement, object dialogIdentifier)
    {
        ArgumentNullException.ThrowIfNull(childElement);

        DialogHost? dialogHost = childElement.GetVisualAncestry()
            .OfType<DialogHost>()
            .FirstOrDefault(x => x.Identifier is not null && x.Identifier.Equals(dialogIdentifier));

        if (dialogHost is null)
        {
            throw new InvalidOperationException($"Unable to find a DialogHost in visual tree ancestry with identifier {dialogIdentifier}");
        }

        return dialogHost;
    }
}
