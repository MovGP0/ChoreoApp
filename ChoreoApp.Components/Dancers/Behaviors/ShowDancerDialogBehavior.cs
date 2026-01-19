using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Dancers.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class ShowDancerDialogBehavior(
    ISubscriber<ShowDancerDialogCommand> subscriber,
    ILogger<DancerSettingsViewModel> logger):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ShowDancerDialogBehavior), nameof(DancerSettingsViewModel));
        subscriber
            .Subscribe(command => ShowDialog(viewModel, command))
            .DisposeWith(disposables);
    }

    private static void ShowDialog(DancerSettingsViewModel viewModel, ShowDancerDialogCommand command)
    {
        viewModel.DialogContentView = command.Content;
        viewModel.IsDialogOpen = command.Content is not null;
    }
}
