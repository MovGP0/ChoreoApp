using Plugin.Maui.Audio;

namespace ChoreoApp.AudioPlayer;

public sealed partial class AudioPlayerViewModel : ReactiveObject, IActivatableViewModel
{
    private const double DefaultPreparationSeconds = 4d;

    public AudioPlayerViewModel(IEnumerable<IBehavior<AudioPlayerViewModel>> behaviors)
    {
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
}
