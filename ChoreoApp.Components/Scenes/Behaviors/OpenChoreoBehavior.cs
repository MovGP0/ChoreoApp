using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.AudioPlayer;
using ChoreoApp.Models;
using ChoreoMasterMobile.Json;
using MessagePipe;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class OpenChoreoBehavior(
    Global.GlobalStateModel globalState,
    Floor.IFloorRenderGate renderGate,
    IPreferences preferences,
    IPublisher<OpenAudioFileCommand> openAudioPublisher,
    IPublisher<CloseAudioFileCommand> closeAudioPublisher) : IBehavior<ScenesPaneViewModel>
{
    private static readonly ChoreographyModelMapper Mapper = new();

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
        await renderGate.WaitForFirstRenderAsync(cancellationToken);

        var storedPath = preferences.Get(SettingsPreferenceKeys.LastOpenedChoreoFile, string.Empty);
        if (string.IsNullOrWhiteSpace(storedPath) || !File.Exists(storedPath))
        {
            return;
        }

        await LoadChoreoAsync(storedPath, cancellationToken);
    }

    private async Task LoadChoreoAsync(string path, CancellationToken cancellationToken = default)
    {
        var choreography = await Task.Run(() => Util.ImportFromFile(path), cancellationToken);
        var mapped = Mapper.Map(choreography);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            globalState.Choreography = mapped;
            preferences.Set(SettingsPreferenceKeys.LastOpenedChoreoFile, path);
        });

        await TryLoadAudioAsync(path, mapped.Settings, cancellationToken);
    }

    private async Task TryLoadAudioAsync(string choreographyFilePath, SettingsModel? settings, CancellationToken cancellationToken)
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

        if (!candidates.Any())
        {
            var storedAudioPath = preferences.Get(SettingsPreferenceKeys.LastOpenedAudioFile, string.Empty);
            if (!string.IsNullOrWhiteSpace(storedAudioPath))
            {
                candidates.Add(storedAudioPath);
            }
        }

        foreach (var candidate in candidates.Where(File.Exists))
        {
            openAudioPublisher.Publish(new OpenAudioFileCommand(candidate));
            return;
        }

        closeAudioPublisher.Publish(new CloseAudioFileCommand());
    }
}
