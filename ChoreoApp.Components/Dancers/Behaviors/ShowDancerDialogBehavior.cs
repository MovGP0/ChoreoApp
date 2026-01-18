using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Dancers.Messages;
using MessagePipe;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class ShowDancerDialogBehavior(
    ISubscriber<ShowDancerDialogCommand> subscriber):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
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
