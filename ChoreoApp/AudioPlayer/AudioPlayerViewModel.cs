using Plugin.Maui.Audio;

namespace ChoreoApp.AudioPlayer;

public sealed partial class AudioPlayerViewModel : ReactiveObject, IActivatableViewModel
{
    private const double DefaultPreparationSeconds = 4d;
    private readonly IHapticFeedback _hapticFeedback;

    public AudioPlayerViewModel(
        IEnumerable<IBehavior<AudioPlayerViewModel>> behaviors,
        IHapticFeedback hapticFeedback)
    {
        _hapticFeedback = hapticFeedback;

        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private double _speed = 1d;

    [Reactive]
    private double _minimumSpeed = 0.8d;

    [Reactive]
    private double _maximumSpeed = 1.1d;

    [Reactive]
    private double _volume = 1d;

    [Reactive]
    private double _balance;

    [Reactive]
    private double _duration;

    [Reactive]
    private double _position;

    [Reactive]
    private string _tickValues = string.Empty;

    [Reactive]
    private bool _canLinkSceneToPosition;

    [Reactive]
    private bool _isPlaying;

    [Reactive]
    private bool _loop;

    [Reactive]
    private bool _canSeek;

    [Reactive]
    private bool _canSetSpeed;

    [Reactive]
    private Func<Task<Stream>>? _streamFactory;

    [Reactive]
    private double _preparationSeconds = DefaultPreparationSeconds;

    [Reactive]
    private double _pauseSeconds;

    [Reactive]
    private string _title = "Audio";

    internal IAudioPlayer? Player { get; set; }

    [ReactiveCommand]
    private Task TogglePlayPauseAsync()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        var player = Player;

        if (player is null)
        {
            return Task.CompletedTask;
        }

        if (player.IsPlaying)
        {
            player.Pause();
            IsPlaying = false;
            return Task.CompletedTask;
        }

        player.Play();
        IsPlaying = true;

        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task StopAsync()
    {
        var player = Player;

        if (player is null)
        {
            return Task.CompletedTask;
        }

        player.Stop();
        IsPlaying = false;
        Position = 0d;

        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task SeekAsync(double position)
    {
        var player = Player;

        if (player is null || !player.CanSeek)
        {
            return Task.CompletedTask;
        }

        player.Seek(position);
        Position = player.CurrentPosition;

        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task ReloadAsync()
    {
        // Handled by the behavior when the StreamFactory is set.
        return Task.CompletedTask;
    }

    [ReactiveCommand(CanExecute = nameof(CanLinkSceneToPosition))]
    private Task LinkSceneToPositionAsync()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        // Handled by the link behavior.
        return Task.CompletedTask;
    }
}
