using System.Reactive.Disposables.Fluent;
using ChoreoApp.AudioPlayer;
using ChoreoApp.i18n;
using MessagePipe;

namespace ChoreoApp.Main;

public sealed partial class MainViewModel : ReactiveObject, IActivatableViewModel
{
    public MainViewModel(
        IEnumerable<IBehavior<MainViewModel>> behaviors,
        AudioPlayerViewModel audioPlayerViewModel,
        IPublisher<OpenAudioFileCommand> openAudioPublisher)
    {
        AudioPlayerViewModel = audioPlayerViewModel;
        _openAudioPublisher = openAudioPublisher;

        this.WhenActivated(disposables =>
        {
            audioPlayerViewModel.Activator.Activate().DisposeWith(disposables);
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    private const double DefaultNavWidth = 280d;
    private readonly IPublisher<OpenAudioFileCommand> _openAudioPublisher;

    public ViewModelActivator Activator { get; } = new();
    public AudioPlayerViewModel AudioPlayerViewModel { get; }

    [Reactive]
    private GridLength _navColumnWidth = new(DefaultNavWidth);

    [Reactive]
    private bool _isNavOpen = true;

    [Reactive]
    private string _title = Translations.AppTitle;

    [Reactive]
    private string _selectedSceneName = string.Empty;

    [Reactive]
    private bool _isAudioPlayerOpen;

    [ReactiveCommand]
    private async Task ToggleAudioPlayer()
    {
        if (IsAudioPlayerOpen)
        {
            IsAudioPlayerOpen = false;
            return;
        }

        if (AudioPlayerViewModel.StreamFactory is not null)
        {
            IsAudioPlayerOpen = true;
            return;
        }

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

        _openAudioPublisher.Publish(new OpenAudioFileCommand(path));
        IsAudioPlayerOpen = true;
    }

    public void ToggleNavigation()
    {
        IsNavOpen = !IsNavOpen;
        NavColumnWidth = IsNavOpen ? new GridLength(DefaultNavWidth) : new GridLength(0);
    }
}
