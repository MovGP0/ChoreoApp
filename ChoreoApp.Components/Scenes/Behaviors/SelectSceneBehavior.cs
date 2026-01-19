using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes.Events;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class SelectSceneBehavior(
    ISubscriber<SceneSelectedEvent> sceneSelectedSubscriber,
    IPublisher<SelectedSceneChangedEvent> selectedSceneChangedPublisher,
    ILogger<ScenesPaneViewModel> logger):
    IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(SelectSceneBehavior), nameof(ScenesPaneViewModel));
        sceneSelectedSubscriber
            .Subscribe(evnt => viewModel.SelectedScene = evnt.SelectedScene)
            .DisposeWith(disposables);

        SceneViewModel? previous = null;

        viewModel
            .WhenAnyValue(vm => vm.SelectedScene)
            .Subscribe(current =>
            {
                if (ReferenceEquals(previous, current))
                {
                    return;
                }

                if (previous is not null)
                {
                    previous.IsSelected = false;
                }

                if (current is not null)
                {
                    current.IsSelected = true;
                }

                previous = current;

                selectedSceneChangedPublisher.Publish(new SelectedSceneChangedEvent(current));
            })
            .DisposeWith(disposables);
    }
}
