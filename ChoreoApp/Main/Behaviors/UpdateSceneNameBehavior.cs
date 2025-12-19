using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes;
using MessagePipe;

namespace ChoreoApp.Main.Behaviors;

public sealed class UpdateSceneNameBehavior(
    ISubscriber<SelectedSceneChangedEvent> subscriber): IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe(scene => viewModel.SelectedSceneName = scene.SelectedScene?.Name ?? string.Empty)
            .DisposeWith(disposables);
    }
}
