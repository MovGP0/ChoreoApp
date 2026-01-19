using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class CloseAudioFileBehavior(
    ISubscriber<CloseAudioFileCommand> subscriber,
    ILogger<AudioPlayerViewModel> logger):
    IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(CloseAudioFileBehavior), nameof(AudioPlayerViewModel));
        subscriber
            .Subscribe(message => HandleClose(viewModel, message))
            .DisposeWith(disposables);
    }

    private static void HandleClose(
        AudioPlayerViewModel viewModel,
        CloseAudioFileCommand command)
    {
        if (viewModel.Player is IDisposable disposablePlayer)
        {
            disposablePlayer.Dispose();
        }

        viewModel.Player = null;
        viewModel.StreamFactory = null;
        viewModel.Title = "Audio";
        viewModel.Position = 0d;
        viewModel.Duration = 0d;
        viewModel.IsPlaying = false;
        viewModel.CanSeek = false;
        viewModel.CanSetSpeed = false;
    }
}
