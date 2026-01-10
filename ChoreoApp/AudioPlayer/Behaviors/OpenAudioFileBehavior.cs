using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Models;
using ChoreoApp.Settings;
using MessagePipe;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class OpenAudioFileBehavior(
    ISubscriber<OpenAudioFileCommand> subscriber):
    IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe(message => HandleOpen(viewModel, message))
            .DisposeWith(disposables);
    }

    private static void HandleOpen(
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
        Preferences.Default.Set(SettingsPreferenceKeys.LastOpenedAudioFile, filePath);
    }
}
