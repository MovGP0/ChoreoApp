namespace ChoreoApp.AudioPlayer;

public partial class AudioPlayerView
{
    private const double SpeedSnapStep = 0.05d;
    private bool _isUserDragging;
    private bool _wasPlaying;
    private bool _isAdjustingSpeed;

    public AudioPlayerView()
    {
        InitializeComponent();
    }

    private void OnPositionDragStarted(object sender, EventArgs e)
    {
        _isUserDragging = true;
        _wasPlaying = false;

        if (BindingContext is not AudioPlayerViewModel viewModel)
        {
            return;
        }

        var player = viewModel.Player;

        if (player is null || !player.IsPlaying)
        {
            return;
        }

        player.Pause();
        viewModel.IsPlaying = false;
        _wasPlaying = true;
    }

    private void OnPositionDragCompleted(object sender, EventArgs e)
    {
        _isUserDragging = false;

        if (BindingContext is not AudioPlayerViewModel viewModel)
        {
            return;
        }

        if (!viewModel.CanSeek)
        {
            return;
        }

        if (sender is not Slider slider)
        {
            return;
        }

        viewModel.SeekCommand.Execute(slider.Value).Subscribe();

        if (!_wasPlaying)
        {
            return;
        }

        var player = viewModel.Player;

        if (player is null)
        {
            return;
        }

        player.Play();
        viewModel.IsPlaying = true;
    }

    private void OnPositionChanged(object sender, ValueChangedEventArgs e)
    {
        if (!_isUserDragging)
        {
            return;
        }

        if (BindingContext is not AudioPlayerViewModel viewModel)
        {
            return;
        }

        if (!viewModel.CanSeek)
        {
            return;
        }
    }

    private void OnSpeedChanged(object sender, ValueChangedEventArgs e)
    {
        if (_isAdjustingSpeed)
        {
            return;
        }

        if (BindingContext is not AudioPlayerViewModel viewModel)
        {
            return;
        }

        var snapped = Math.Round(e.NewValue / SpeedSnapStep, MidpointRounding.AwayFromZero) * SpeedSnapStep;
        snapped = Math.Clamp(snapped, viewModel.MinimumSpeed, viewModel.MaximumSpeed);

        if (Math.Abs(snapped - e.NewValue) < 0.0001d)
        {
            return;
        }

        _isAdjustingSpeed = true;
        viewModel.Speed = snapped;
        _isAdjustingSpeed = false;
    }
}
