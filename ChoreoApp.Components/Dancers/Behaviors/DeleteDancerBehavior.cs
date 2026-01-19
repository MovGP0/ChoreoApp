using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class DeleteDancerBehavior(
    ILogger<DancerSettingsViewModel> logger):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(DeleteDancerBehavior), nameof(DancerSettingsViewModel));
        viewModel.DeleteDancerCommand
            .Subscribe(_ => DeleteDancer(viewModel))
            .DisposeWith(disposables);
    }

    private static void DeleteDancer(DancerSettingsViewModel viewModel)
    {
        if (viewModel.SelectedDancer is null)
        {
            return;
        }

        var index = viewModel.Dancers.IndexOf(viewModel.SelectedDancer);
        if (index < 0)
        {
            return;
        }

        viewModel.Dancers.RemoveAt(index);
        viewModel.SelectedDancer = viewModel.Dancers.Count == 0
            ? null
            : viewModel.Dancers[Math.Min(index, viewModel.Dancers.Count - 1)];
    }
}
