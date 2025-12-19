using DynamicData.Binding;

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
    [Reactive] private string _text = "";
    [Reactive] private bool _fixedPositions;
    [Reactive] private TimeSpan? _timestamp;
    [Reactive] private bool _isSelected;

    [ReactiveCollection]
    private ObservableCollectionExtended<ChoreoMasterMobile.Json.Position> _positions = new();

    [Reactive] private int _variationDepth;
    [Reactive] private IList<IList<ChoreoMasterMobile.Json.Scene>>? _variations;
    [Reactive] private IList<ChoreoMasterMobile.Json.Scene>? _currentVariation;
    [Reactive] private Color _color = Colors.Transparent;

    [ReactiveCommand]
    private void SelectScene()
    {
        IsSelected = true;
    }

    public ViewModelActivator Activator { get; } = new();
}
