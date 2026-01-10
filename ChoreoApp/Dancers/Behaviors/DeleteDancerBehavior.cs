using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class DeleteDancerBehavior : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
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