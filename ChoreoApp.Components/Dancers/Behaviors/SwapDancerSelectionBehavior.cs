using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SwapDancerSelectionBehavior : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        EnsureSwapSelections(viewModel);
        UpdateCanSwap(viewModel);

        viewModel
            .WhenAnyValue(vm => vm.SwapFromDancer, vm => vm.SwapToDancer)
            .Subscribe(_ => UpdateCanSwap(viewModel))
            .DisposeWith(disposables);

        Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => viewModel.Dancers.CollectionChanged += handler,
                handler => viewModel.Dancers.CollectionChanged -= handler)
            .Subscribe(_ => EnsureSwapSelections(viewModel))
            .DisposeWith(disposables);
    }

    private static void EnsureSwapSelections(DancerSettingsViewModel viewModel)
    {
        var dancers = viewModel.Dancers;
        if (dancers.Count == 0)
        {
            viewModel.SwapFromDancer = null;
            viewModel.SwapToDancer = null;
            UpdateCanSwap(viewModel);
            return;
        }

        if (viewModel.SwapFromDancer is null || !dancers.Contains(viewModel.SwapFromDancer))
        {
            viewModel.SwapFromDancer = dancers[0];
        }

        if (dancers.Count < 2)
        {
            viewModel.SwapToDancer = null;
            UpdateCanSwap(viewModel);
            return;
        }

        if (viewModel.SwapToDancer is null
            || !dancers.Contains(viewModel.SwapToDancer)
            || ReferenceEquals(viewModel.SwapFromDancer, viewModel.SwapToDancer))
        {
            viewModel.SwapToDancer = dancers.FirstOrDefault(dancer => !ReferenceEquals(dancer, viewModel.SwapFromDancer));
        }

        UpdateCanSwap(viewModel);
    }

    private static void UpdateCanSwap(DancerSettingsViewModel viewModel)
    {
        viewModel.CanSwapDancers = viewModel.SwapFromDancer is not null
                                   && viewModel.SwapToDancer is not null
                                   && !ReferenceEquals(viewModel.SwapFromDancer, viewModel.SwapToDancer);
    }
}
