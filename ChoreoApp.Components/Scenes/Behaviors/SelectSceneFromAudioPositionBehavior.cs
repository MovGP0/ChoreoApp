using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.AudioPlayer.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class SelectSceneFromAudioPositionBehavior(
    ISubscriber<AudioPlayerPositionChangedEvent> audioPositionChangedSubscriber,
    ILogger<ScenesPaneViewModel> logger)
    : IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(SelectSceneFromAudioPositionBehavior), nameof(ScenesPaneViewModel));
        audioPositionChangedSubscriber
            .AsObservable()
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(evt => UpdateSelection(viewModel, evt.PositionSeconds))
            .DisposeWith(disposables);
    }

    private static void UpdateSelection(ScenesPaneViewModel viewModel, double positionSeconds)
    {
        var scenes = viewModel.Scenes;
        if (scenes.Count == 0)
        {
            return;
        }

        SceneViewModel? firstSceneWithTimestamp = null;
        TimeSpan? firstTimestamp = null;
        int firstTimestampIndex = -1;

        for (int index = 0; index < scenes.Count; index++)
        {
            var timestamp = scenes[index].Timestamp;
            if (timestamp.HasValue)
            {
                firstSceneWithTimestamp = scenes[index];
                firstTimestamp = timestamp;
                firstTimestampIndex = index;
                break;
            }
        }

        if (firstSceneWithTimestamp is null || firstTimestamp is null)
        {
            return;
        }

        if (positionSeconds < firstTimestamp.Value.TotalSeconds)
        {
            SelectIfChanged(viewModel, firstSceneWithTimestamp);
            return;
        }

        for (int index = firstTimestampIndex; index < scenes.Count; index++)
        {
            var currentScene = scenes[index];
            if (currentScene.Timestamp is not { } currentTimestamp)
            {
                continue;
            }

            int nextIndex = index + 1;
            if (nextIndex >= scenes.Count)
            {
                return;
            }

            var nextScene = scenes[nextIndex];
            if (nextScene.Timestamp is not { } nextTimestamp)
            {
                continue;
            }

            double currentSeconds = currentTimestamp.TotalSeconds;
            double nextSeconds = nextTimestamp.TotalSeconds;

            if (positionSeconds >= currentSeconds && positionSeconds < nextSeconds)
            {
                SelectIfChanged(viewModel, currentScene);
                return;
            }
        }
    }

    private static void SelectIfChanged(ScenesPaneViewModel viewModel, SceneViewModel scene)
    {
        if (ReferenceEquals(viewModel.SelectedScene, scene))
        {
            return;
        }

        viewModel.SelectedScene = scene;
    }
}
