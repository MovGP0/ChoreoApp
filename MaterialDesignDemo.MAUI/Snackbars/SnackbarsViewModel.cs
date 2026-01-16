using MaterialDesignThemes.Maui;

namespace MaterialDesignDemo.Maui.Snackbars;

public sealed partial class SnackbarsViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    public SnackbarsViewModel()
    {
        MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(4));
    }

    public ViewModelActivator Activator { get; } = new();

    public SnackbarMessageQueue MessageQueue { get; }

    [Reactive]
    private bool _isSimpleActive;

    [Reactive]
    private bool _isActionActive;

    [Reactive]
    private string _messageText = "Hello World";

    [ReactiveCommand]
    private void SendQueued()
    {
        if (string.IsNullOrWhiteSpace(MessageText))
        {
            return;
        }

        MessageQueue.Enqueue(MessageText, "UNDO", () => { });
    }

    public void Dispose()
    {
        MessageQueue.Dispose();
    }
}
