using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Dancers.Messages;
using MessagePipe;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class HideDancerDialogBehavior(
    ISubscriber<CloseDancerDialogCommand> subscriber):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
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
