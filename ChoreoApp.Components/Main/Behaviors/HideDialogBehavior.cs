using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Main.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Main.Behaviors;

public sealed class HideDialogBehavior(
    ISubscriber<CloseDialogCommand> subscriber,
    ILogger<MainViewModel> logger):
    IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(HideDialogBehavior), nameof(MainViewModel));
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
