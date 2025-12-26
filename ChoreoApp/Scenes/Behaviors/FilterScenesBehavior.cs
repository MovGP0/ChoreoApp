using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Collections.Specialized;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class FilterScenesBehavior(Global.GlobalStateModel globalState)
    : IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => globalState.Scenes.CollectionChanged += h,
                h => globalState.Scenes.CollectionChanged -= h)
            .Subscribe(_ => viewModel.RefreshScenes())
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SearchText)
            .Subscribe(_ =>
            {
                viewModel.RefreshScenes();
            })
            .DisposeWith(disposables);

        viewModel.RefreshScenes();
    }
}
