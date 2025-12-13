using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using MessagePipe;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class OpenAudioFileBehavior(
    IAsyncSubscriber<OpenAudioFileCommand> subscriber):
    IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe((message, ct) => HandleOpenAsync(viewModel, message, ct))
            .DisposeWith(disposables);
    }

    private static async ValueTask HandleOpenAsync(
        AudioPlayerViewModel viewModel,
        OpenAudioFileCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.FilePath))
        {
            return;
        }

        var filePath = command.FilePath;
        viewModel.Title = Path.GetFileName(filePath);

        viewModel.StreamFactory = async () =>
        {
            await Task.CompletedTask;
            return File.OpenRead(filePath);
        };
    }
}
