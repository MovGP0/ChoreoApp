using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes.Events;
using MessagePipe;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class PublishSceneSelectedBehavior(
    IPublisher<SceneSelectedEvent> sceneSelectedPublisher):
    IBehavior<SceneViewModel>
{
    public void Activate(SceneViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.SelectSceneCommand
            .Subscribe(_ => sceneSelectedPublisher.Publish(new(viewModel)))
            .DisposeWith(disposables);
    }
}
