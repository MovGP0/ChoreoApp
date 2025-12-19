using System.Collections.Specialized;
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class AudioPlayerTicksBehavior(GlobalStateModel globalState) : IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        var sceneTimestampDisposable = new SerialDisposable().DisposeWith(disposables);

        RefreshSceneSubscriptions();
        UpdateTicks(viewModel);

        var scenesChanged = Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => globalState.Scenes.CollectionChanged += h,
                h => globalState.Scenes.CollectionChanged -= h)
            .Select(_ => Unit.Default);

        scenesChanged
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                RefreshSceneSubscriptions();
                UpdateTicks(viewModel);
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.Duration)
            .Skip(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdateTicks(viewModel))
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.StreamFactory)
            .Skip(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdateTicks(viewModel))
            .DisposeWith(disposables);

        void RefreshSceneSubscriptions()
        {
            var inner = new CompositeDisposable();
            foreach (var scene in globalState.Scenes)
            {
                scene
                    .WhenAnyValue(vm => vm.Timestamp)
                    .Skip(1)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => UpdateTicks(viewModel))
                    .DisposeWith(inner);
            }

            sceneTimestampDisposable.Disposable = inner;
        }
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
}
