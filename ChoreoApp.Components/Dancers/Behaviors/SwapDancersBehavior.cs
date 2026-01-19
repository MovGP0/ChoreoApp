using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Dancers.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SwapDancersBehavior(
    IHapticFeedback hapticFeedback,
    IPublisher<ShowDancerDialogCommand> showDialogPublisher,
    IPublisher<CloseDancerDialogCommand> closeDialogPublisher,
    ILogger<DancerSettingsViewModel> logger):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(SwapDancersBehavior), nameof(DancerSettingsViewModel));
        viewModel.SwapDancersCommand
            .Subscribe(_ => ShowSwapDialog(viewModel))
            .DisposeWith(disposables);
    }

    private void ShowSwapDialog(DancerSettingsViewModel viewModel)
    {
        if (viewModel.SwapFromDancer is null || viewModel.SwapToDancer is null)
        {
            return;
        }

        if (ReferenceEquals(viewModel.SwapFromDancer, viewModel.SwapToDancer))
        {
            return;
        }

        var dialogViewModel = new SwapDancersDialogViewModel(
            closeDialogPublisher,
            hapticFeedback,
            viewModel.SwapFromDancer,
            viewModel.SwapToDancer);
        var dialogView = new SwapDancersDialogView { ViewModel = dialogViewModel };
        showDialogPublisher.Publish(new ShowDancerDialogCommand(dialogView));
    }
}
