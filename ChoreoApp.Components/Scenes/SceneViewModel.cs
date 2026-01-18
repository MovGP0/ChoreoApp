using ChoreoApp.Models;
using DynamicData.Binding;

namespace ChoreoApp.Scenes;

public sealed partial class SceneViewModel: ReactiveObject, IActivatableViewModel
{
    private readonly IHapticFeedback _hapticFeedback;

    public SceneViewModel(
        IEnumerable<IBehavior<SceneViewModel>> behaviors,
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

    [Reactive] private ChoreoMasterMobile.Json.SceneId _sceneId;
    [Reactive] private string _name = "";
    [Reactive] private string _text = "";
    [Reactive] private bool _fixedPositions;
    [Reactive] private TimeSpan? _timestamp;
    [Reactive] private bool _isSelected;

    [ReactiveCollection]
    private ObservableCollectionExtended<PositionModel> _positions = new();

    [Reactive] private int _variationDepth;
    [Reactive] private IList<IList<SceneModel>>? _variations;
    [Reactive] private IList<SceneModel>? _currentVariation;
    [Reactive] private Color _color = Colors.Transparent;

    [ReactiveCommand]
    private void SelectScene()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        IsSelected = true;
    }

    public ViewModelActivator Activator { get; } = new();
}
