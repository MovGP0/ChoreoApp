using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes.Events;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class PublishSceneSelectedBehavior(
    IPublisher<SceneSelectedEvent> sceneSelectedPublisher,
    ILogger<SceneViewModel> logger):
    IBehavior<SceneViewModel>
{
    public void Activate(SceneViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(PublishSceneSelectedBehavior), nameof(SceneViewModel));
        viewModel.SelectSceneCommand
            .Subscribe(_ => sceneSelectedPublisher.Publish(new(viewModel)))
            .DisposeWith(disposables);
    }
}
