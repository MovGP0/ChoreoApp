using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Main.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Main.Behaviors;

public sealed class ShowDialogBehavior(
    ISubscriber<ShowDialogCommand> subscriber,
    ILogger<MainViewModel> logger):
    IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ShowDialogBehavior), nameof(MainViewModel));
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
