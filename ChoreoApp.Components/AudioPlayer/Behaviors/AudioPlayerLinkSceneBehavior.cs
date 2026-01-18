using System.Collections.Specialized;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;
using ChoreoApp.Scenes;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class AudioPlayerLinkSceneBehavior(GlobalStateModel globalState) : IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        var sceneTimestampDisposable = new SerialDisposable().DisposeWith(disposables);

        viewModel
            .LinkSceneToPositionCommand
            .Subscribe(_ => LinkSceneToPosition(viewModel))
            .DisposeWith(disposables);

        globalState
            .WhenAnyValue(gs => gs.SelectedScene)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                RefreshSceneSubscriptions();
                UpdateCanLink(viewModel);
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.Position)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdateCanLink(viewModel))
            .DisposeWith(disposables);

        Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => globalState.Scenes.CollectionChanged += h,
                h => globalState.Scenes.CollectionChanged -= h)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                RefreshSceneSubscriptions();
                UpdateCanLink(viewModel);
            })
            .DisposeWith(disposables);

        RefreshSceneSubscriptions();
        UpdateCanLink(viewModel);

        void RefreshSceneSubscriptions()
        {
            var inner = new CompositeDisposable();
            foreach (var scene in globalState.Scenes)
            {
                scene
                    .WhenAnyValue(vm => vm.Timestamp)
                    .Skip(1)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => UpdateCanLink(viewModel))
                    .DisposeWith(inner);
            }

            sceneTimestampDisposable.Disposable = inner;
        }
    }

    private void LinkSceneToPosition(AudioPlayerViewModel viewModel)
    {
        var selectedScene = globalState.SelectedScene;

        if (selectedScene is null)
        {
            return;
        }

        if (!TryGetLinkedTimestamp(viewModel, selectedScene, out var roundedTimestamp))
        {
            return;
        }

        selectedScene.Timestamp = roundedTimestamp;

        if (globalState.Choreography is not { } choreography)
        {
            UpdateTicks(viewModel);
            return;
        }

        var modelScene = choreography.Scenes.FirstOrDefault(scene => scene.SceneId == selectedScene.SceneId)
            ?? choreography.Scenes.FirstOrDefault(scene => string.Equals(scene.Name, selectedScene.Name, StringComparison.Ordinal));

        if (modelScene is not null)
        {
            modelScene.Timestamp = roundedTimestamp;
        }

        UpdateTicks(viewModel);
    }

    private void UpdateCanLink(AudioPlayerViewModel viewModel)
    {
        var selectedScene = globalState.SelectedScene;

        if (selectedScene is null)
        {
            viewModel.CanLinkSceneToPosition = false;
            return;
        }

        viewModel.CanLinkSceneToPosition = TryGetLinkedTimestamp(viewModel, selectedScene, out _);
    }

    private bool TryGetLinkedTimestamp(
        AudioPlayerViewModel viewModel,
        SceneViewModel selectedScene,
        out TimeSpan? roundedTimestamp)
    {
        roundedTimestamp = null;

        var scenes = globalState.Scenes;
        var selectedIndex = scenes.IndexOf(selectedScene);

        if (selectedIndex < 0)
        {
            return false;
        }

        var beforeTimestamp = selectedIndex > 0
            ? scenes[selectedIndex - 1].Timestamp
            : null;

        var afterTimestamp = selectedIndex < scenes.Count - 1
            ? scenes[selectedIndex + 1].Timestamp
            : null;

        roundedTimestamp = RoundTo100Milliseconds(viewModel.Position);

        if (beforeTimestamp.HasValue && roundedTimestamp <= beforeTimestamp.Value)
        {
            return false;
        }

        if (afterTimestamp.HasValue && roundedTimestamp >= afterTimestamp.Value)
        {
            return false;
        }

        return true;
    }

    private void UpdateTicks(AudioPlayerViewModel viewModel)
    {
        var max = viewModel.Duration;
        var ticks = globalState.Scenes
            .Select(scene => scene.Timestamp)
            .Where(timestamp => timestamp.HasValue)
            .Select(timestamp => timestamp!.Value.TotalSeconds)
            .Where(value => max <= 0d || value <= max)
            .OrderBy(value => value)
            .Distinct()
            .Select(value => value.ToString("0.###", CultureInfo.InvariantCulture))
            .ToArray();

        viewModel.TickValues = ticks.Length == 0
            ? string.Empty
            : string.Join(",", ticks);
    }

    private static TimeSpan RoundTo100Milliseconds(double seconds)
    {
        var milliseconds = seconds * 1000d;
        var rounded = Math.Round(milliseconds / 100d, MidpointRounding.AwayFromZero) * 100d;
        return TimeSpan.FromMilliseconds(rounded);
    }
}
