using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Dancers.Messages;
using MessagePipe;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SwapDancersBehavior(
    IPublisher<ShowDancerDialogCommand> showDialogPublisher,
    IPublisher<CloseDancerDialogCommand> closeDialogPublisher):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
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
            viewModel.SwapFromDancer,
            viewModel.SwapToDancer);
        var dialogView = new SwapDancersDialogView { ViewModel = dialogViewModel };
        showDialogPublisher.Publish(new ShowDancerDialogCommand(dialogView));
    }
}
