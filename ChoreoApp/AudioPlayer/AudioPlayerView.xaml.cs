namespace ChoreoApp.AudioPlayer;

public partial class AudioPlayerView
{
    public AudioPlayerView()
    {
        InitializeComponent();
    }

    private void OnPositionChanged(object sender, ValueChangedEventArgs e)
    {
        if (BindingContext is not AudioPlayerViewModel viewModel)
        {
            return;
        }

        if (!viewModel.CanSeek)
        {
            return;
        }

        viewModel.Position = e.NewValue;
        viewModel.SeekCommand.Execute(e.NewValue).Subscribe();
    }
}
