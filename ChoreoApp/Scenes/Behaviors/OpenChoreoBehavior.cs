using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.AudioPlayer;
using ChoreoMasterMobile.Json;
using MessagePipe;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class OpenChoreoBehavior(
    Global.GlobalStateModel globalState,
    IAsyncPublisher<OpenAudioFileCommand> openAudioPublisher,
    IAsyncPublisher<CloseAudioFileCommand> closeAudioPublisher) : IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .OpenChoreoCommand
            .Subscribe(async _ => await HandleOpenAsync())
            .DisposeWith(disposables);
    }

    private async Task HandleOpenAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Open choreography file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                [DevicePlatform.WinUI] = [".choreo"],
                [DevicePlatform.MacCatalyst] = ["choreo"],
                [DevicePlatform.iOS] = ["choreo"],
                [DevicePlatform.Android] = ["application/octet-stream", "application/json", "*/*"],
            })
        });

        if (result is null)
        {
            return;
        }

        if (!string.Equals(Path.GetExtension(result.FileName), ".choreo", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unsupported file type: {result.FileName}");
        }

        var path = result.FullPath;

        if (string.IsNullOrWhiteSpace(path))
        {
            await using var pickedStream = await result.OpenReadAsync();
            var tempPath = Path.Combine(FileSystem.CacheDirectory, $"{Path.GetFileNameWithoutExtension(result.FileName)}.choreo");
            await using var tempFile = File.Open(tempPath, FileMode.Create, FileAccess.Write);
            await pickedStream.CopyToAsync(tempFile);
            path = tempPath;
        }

        var choreography = Util.ImportFromFile(path);
        globalState.Choreography = choreography;

        await TryLoadAudioAsync(path, choreography.Settings);
    }

    private async Task TryLoadAudioAsync(string choreographyFilePath, ChoreoMasterMobile.Json.Settings? settings)
    {
        if (settings is null)
        {
            return;
        }

        var candidates = new List<string>();

        if (!string.IsNullOrWhiteSpace(settings.MusicPathAbsolute))
        {
            candidates.Add(settings.MusicPathAbsolute);
        }

        if (!string.IsNullOrWhiteSpace(settings.MusicPathRelative))
        {
            var baseDir = Path.GetDirectoryName(choreographyFilePath) ?? string.Empty;
            candidates.Add(Path.Combine(baseDir, settings.MusicPathRelative));
        }

        foreach (var candidate in candidates.Where(File.Exists))
        {
            await openAudioPublisher.PublishAsync(new OpenAudioFileCommand(candidate));
            return;
        }

        await closeAudioPublisher.PublishAsync(new CloseAudioFileCommand());
    }
}
