using ChoreoApp.i18n;

namespace ChoreoApp.Main;

public sealed partial class MainViewModel : ReactiveObject, IActivatableViewModel
{
    public MainViewModel(
        IEnumerable<IBehavior<MainViewModel>> behaviors)
    {
        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    private const double DefaultNavWidth = 280d;

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private GridLength _navColumnWidth = new(DefaultNavWidth);

    [Reactive]
    private bool _isNavOpen = true;

    [Reactive]
    private string _title = Translations.AppTitle;

    [Reactive]
    private bool _isAudioPlayerOpen;

    [ReactiveCommand]
    private void ToggleAudioPlayer()
    {
        IsAudioPlayerOpen = !IsAudioPlayerOpen;
    }

    public void ToggleNavigation()
    {
        IsNavOpen = !IsNavOpen;
        NavColumnWidth = IsNavOpen ? new GridLength(DefaultNavWidth) : new GridLength(0);
    }
}
