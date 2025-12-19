using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.AudioPlayer;
using ChoreoApp.Settings;
using ChoreoMasterMobile.Json;
using MessagePipe;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class OpenChoreoBehavior(
    Global.GlobalStateModel globalState,
    IPublisher<OpenAudioFileCommand> openAudioPublisher,
    IPublisher<CloseAudioFileCommand> closeAudioPublisher) : IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .OpenChoreoCommand
            .Subscribe(async _ => await HandleOpenAsync())
            .DisposeWith(disposables);

        var cancellationTokenSource = new CancellationTokenSource();
        Disposable
            .Create(() => cancellationTokenSource.Cancel())
            .DisposeWith(disposables);

        _ = LoadLastOpenedAsync(cancellationTokenSource.Token);
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

        await LoadChoreoAsync(path);
    }

    private async Task LoadLastOpenedAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);

        var storedPath = Preferences.Default.Get(SettingsPreferenceKeys.LastOpenedChoreoFile, string.Empty);
        if (string.IsNullOrWhiteSpace(storedPath) || !File.Exists(storedPath))
        {
            return;
        }

        await LoadChoreoAsync(storedPath);
    }

    private async Task LoadChoreoAsync(string path)
    {
        var choreography = Util.ImportFromFile(path);
        globalState.Choreography = choreography;
        Preferences.Default.Set(SettingsPreferenceKeys.LastOpenedChoreoFile, path);

        await TryLoadAudioAsync(path, choreography.Settings);
    }

    private Task TryLoadAudioAsync(string choreographyFilePath, ChoreoMasterMobile.Json.Settings? settings)
    {
        if (settings is null)
        {
            return Task.CompletedTask;
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
            openAudioPublisher.Publish(new OpenAudioFileCommand(candidate));
            return Task.CompletedTask;
        }

        closeAudioPublisher.Publish(new CloseAudioFileCommand());
        return Task.CompletedTask;
    }
}
