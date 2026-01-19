using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.AudioPlayer.Messages;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class AudioPlayerPositionChangedBehavior(
    IPublisher<AudioPlayerPositionChangedEvent> publisher,
    ILogger<AudioPlayerViewModel> logger)
    : IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(AudioPlayerPositionChangedBehavior), nameof(AudioPlayerViewModel));
        viewModel
            .WhenAnyValue(vm => vm.Position)
            .Skip(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(seconds => publisher.Publish(new AudioPlayerPositionChangedEvent(seconds)))
            .DisposeWith(disposables);
    }
}
