namespace ChoreoApp.AudioPlayer;

using System.Reactive.Disposables.Fluent;

public partial class AudioPlayerPage
{
    public AudioPlayerPage(AudioPlayerViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;

        this.WhenActivated(disposables =>
        {
            viewModel.Activator.Activate()
                .DisposeWith(disposables);
        });
    }
}
