namespace MaterialDesignDemo.Maui.Dialogs;

public sealed partial class DialogsViewModel : ReactiveObject, IActivatableViewModel
{
    public DialogsViewModel()
    {
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private bool _isDialogOpen;

    [Reactive]
    private View? _dialogContent;

    [ReactiveCommand]
    private Task OpenSampleDialogAsync()
    {
        DialogContent = new Views.SampleDialogView();
        IsDialogOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenMessageDialogAsync()
    {
        DialogContent = new Views.SampleMessageDialogView();
        IsDialogOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenProgressDialogAsync()
    {
        DialogContent = new Views.SampleProgressDialogView();
        IsDialogOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenSample4DialogAsync()
    {
        DialogContent = new Views.Sample4DialogView();
        IsDialogOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task CloseDialogAsync()
    {
        IsDialogOpen = false;
        return Task.CompletedTask;
    }
}
