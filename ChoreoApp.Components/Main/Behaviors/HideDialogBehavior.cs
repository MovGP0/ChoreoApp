using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Main.Messages;
using MessagePipe;

namespace ChoreoApp.Main.Behaviors;

public sealed class HideDialogBehavior(
    ISubscriber<CloseDialogCommand> subscriber):
    IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe(_ => HideDialog(viewModel))
            .DisposeWith(disposables);
    }

    private static void HideDialog(MainViewModel viewModel)
    {
        viewModel.IsDialogOpen = false;
        viewModel.DialogContentView = null;
    }
}
