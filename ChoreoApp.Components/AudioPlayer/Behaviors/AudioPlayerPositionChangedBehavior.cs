using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.AudioPlayer.Messages;
using MessagePipe;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class AudioPlayerPositionChangedBehavior(
    IPublisher<AudioPlayerPositionChangedEvent> publisher)
    : IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.Position)
            .Skip(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(seconds => publisher.Publish(new AudioPlayerPositionChangedEvent(seconds)))
            .DisposeWith(disposables);
    }
}
