using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Main.Messages;
using MessagePipe;

namespace ChoreoApp.Main.Behaviors;

public sealed class ShowDialogBehavior(
    ISubscriber<ShowDialogCommand> subscriber):
    IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe(command => ShowDialog(viewModel, command))
            .DisposeWith(disposables);
    }

    private static void ShowDialog(MainViewModel viewModel, ShowDialogCommand command)
    {
        viewModel.DialogContentView = command.Content;
        viewModel.IsDialogOpen = command.Content is not null;
    }
}
