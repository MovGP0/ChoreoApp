using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class OpenAudioFileBehavior(
    ISubscriber<OpenAudioFileCommand> subscriber,
    IPreferences preferences):
    IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe(message => HandleOpen(viewModel, message))
            .DisposeWith(disposables);
    }

    private void HandleOpen(
        AudioPlayerViewModel viewModel,
        OpenAudioFileCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.FilePath))
        {
            return;
        }

        var filePath = command.FilePath;
        viewModel.Title = Path.GetFileName(filePath);

        viewModel.StreamFactory = () => Task.FromResult<Stream>(File.OpenRead(filePath));
        preferences.Set(SettingsPreferenceKeys.LastOpenedAudioFile, filePath);
    }
}
