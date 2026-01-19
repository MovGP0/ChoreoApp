using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Dancers.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class HideDancerDialogBehavior(
    ISubscriber<CloseDancerDialogCommand> subscriber,
    ILogger<DancerSettingsViewModel> logger):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(HideDancerDialogBehavior), nameof(DancerSettingsViewModel));
        subscriber
            .Subscribe(_ => HideDialog(viewModel))
            .DisposeWith(disposables);
    }

    private static void HideDialog(DancerSettingsViewModel viewModel)
    {
        viewModel.IsDialogOpen = false;
        viewModel.DialogContentView = null;
    }
}
