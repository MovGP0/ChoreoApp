using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.AudioPlayer;
using ChoreoApp.Global;
using ChoreoApp.i18n;
using ChoreoApp.Main.Messages;
using MessagePipe;

namespace ChoreoApp.Main;

public sealed partial class MainViewModel : ReactiveObject, IActivatableViewModel
{
    public MainViewModel(
        IEnumerable<IBehavior<MainViewModel>> behaviors,
        GlobalStateModel globalState,
        AudioPlayerViewModel audioPlayerViewModel,
        IPublisher<OpenAudioFileCommand> openAudioPublisher,
        IPublisher<OpenSvgFileCommand> openSvgPublisher)
    {
        _globalState = globalState;
        AudioPlayerViewModel = audioPlayerViewModel;
        _openAudioPublisher = openAudioPublisher;
        _openSvgPublisher = openSvgPublisher;

        ModeOptions = BuildModeOptions();
        SelectedModeOption = ModeOptions.FirstOrDefault(option => option.Mode == _globalState.InteractionMode)
            ?? ModeOptions[0];

        this.WhenActivated(disposables =>
        {
            audioPlayerViewModel.Activator.Activate().DisposeWith(disposables);
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }

            _globalState.WhenAnyValue(state => state.InteractionMode)
                .Subscribe(mode => SelectedModeOption = ModeOptions.FirstOrDefault(option => option.Mode == mode) ?? ModeOptions[0])
                .DisposeWith(disposables);

            this.WhenAnyValue(viewModel => viewModel.SelectedModeOption)
                .Where(option => option is not null)
                .Subscribe(option => _globalState.InteractionMode = option!.Mode)
                .DisposeWith(disposables);
        });
    }

    private const double DefaultNavWidth = 280d;
    private readonly IPublisher<OpenAudioFileCommand> _openAudioPublisher;
    private readonly IPublisher<OpenSvgFileCommand> _openSvgPublisher;
    private readonly GlobalStateModel _globalState;

    public ViewModelActivator Activator { get; } = new();
    public AudioPlayerViewModel AudioPlayerViewModel { get; }
    public IReadOnlyList<InteractionModeOption> ModeOptions { get; }

    [Reactive]
    private InteractionModeOption? _selectedModeOption;

    [Reactive]
    private GridLength _navColumnWidth = new(DefaultNavWidth);

    [Reactive]
    private bool _isNavOpen = true;

    [Reactive]
    private bool _isAudioPlayerOpen;

    [Reactive]
    private bool _isChoreographySettingsOpen;

    [Reactive]
    private bool _isDialogOpen;

    [Reactive]
    private View? _dialogContentView;

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

    [ReactiveCommand]
    private async Task OpenImageAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Open SVG image",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                [DevicePlatform.WinUI] = [".svg"],
                [DevicePlatform.MacCatalyst] = ["svg"],
                [DevicePlatform.iOS] = ["svg"],
                [DevicePlatform.Android] = ["image/svg+xml", "image/*", "*/*"],
            })
        });

        if (result is null)
        {
            return;
        }

        if (!string.Equals(Path.GetExtension(result.FileName), ".svg", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unsupported file type: {result.FileName}");
        }

        var path = result.FullPath;
        if (string.IsNullOrWhiteSpace(path))
        {
            await using var pickedStream = await result.OpenReadAsync();
            var tempPath = Path.Combine(FileSystem.CacheDirectory, $"{Path.GetFileNameWithoutExtension(result.FileName)}.svg");
            await using var tempFile = File.Open(tempPath, FileMode.Create, FileAccess.Write);
            await pickedStream.CopyToAsync(tempFile);
            path = tempPath;
        }

        _openSvgPublisher.Publish(new OpenSvgFileCommand(path));
    }

    [ReactiveCommand]
    private void OpenChoreographySettings()
    {
        IsChoreographySettingsOpen = true;
    }

    private static List<InteractionModeOption> BuildModeOptions() => new()
    {
        new(InteractionMode.View, Translations.ModeView),
        new(InteractionMode.Move, Translations.ModeMove),
        new(InteractionMode.RotateAroundCenter, Translations.ModeRotateAroundCenter),
        new(InteractionMode.RotateAroundDancer, Translations.ModeRotateAroundDancer),
        new(InteractionMode.Scale, Translations.ModeScale),
        new(InteractionMode.LineOfSight, Translations.ModeLineOfSight)
    };

    public void ToggleNavigation()
    {
        IsNavOpen = !IsNavOpen;
        NavColumnWidth = IsNavOpen ? new GridLength(DefaultNavWidth) : new GridLength(0);
    }
}
