using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.AudioPlayer;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Main.Behaviors;

public sealed class OpenAudioBehavior(
    IPublisher<OpenAudioFileCommand> publisher,
    ILogger<MainViewModel> logger)
    : IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(OpenAudioBehavior), nameof(MainViewModel));
        viewModel.OpenAudioCommand
            .SelectMany(_ => Observable.FromAsync(() => HandleOpenAsync(viewModel)))
            .Subscribe()
            .DisposeWith(disposables);
    }

    private async Task HandleOpenAsync(MainViewModel viewModel)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Open audio file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                [DevicePlatform.WinUI] = [".mp3"],
                [DevicePlatform.MacCatalyst] = ["mp3"],
                [DevicePlatform.iOS] = ["mp3"],
                [DevicePlatform.Android] = ["audio/mpeg", "audio/*", "*/*"],
            })
        });

        if (result is null)
        {
            return;
        }

        if (!string.Equals(Path.GetExtension(result.FileName), ".mp3", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unsupported file type: {result.FileName}");
        }

        var path = result.FullPath;

        if (string.IsNullOrWhiteSpace(path))
        {
            await using var pickedStream = await result.OpenReadAsync();
            var tempPath = Path.Combine(FileSystem.CacheDirectory, $"{Path.GetFileNameWithoutExtension(result.FileName)}.mp3");
            await using var tempFile = File.Open(tempPath, FileMode.Create, FileAccess.Write);
            await pickedStream.CopyToAsync(tempFile);
            path = tempPath;
        }

        publisher.Publish(new OpenAudioFileCommand(path));
        viewModel.IsAudioPlayerOpen = true;
    }
}
