namespace ChoreoApp.Scenes;

public sealed partial class SceneViewModel: ReactiveObject, IActivatableViewModel
{
    public SceneViewModel(IEnumerable<IBehavior<SceneViewModel>> behaviors)
    {
        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    [Reactive] private ChoreoMasterMobile.Json.SceneId _sceneId;
    [Reactive] private string _name = "";
    [Reactive] private TimeSpan? _timestamp = null;
    [Reactive] private Color _color = Colors.Transparent;
    [Reactive] private bool _isSelected;

    [ReactiveCommand]
    private void SelectScene()
    {
        IsSelected = true;
    }

    public ViewModelActivator Activator { get; } = new();
}
