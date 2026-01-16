using System.Reactive.Disposables;
using System.Reactive.Linq;
using ChoreoApp.Models;
using ChoreoApp.Scenes;
using DynamicData.Binding;

namespace ChoreoApp.Global;

public sealed partial class GlobalStateModel : ReactiveObject
{
    private readonly IReadOnlyList<IBehavior<SettingsModel>> _settingsBehaviors;
    private CompositeDisposable _settingsBehaviorDisposables = new();

    public GlobalStateModel(IEnumerable<IBehavior<SettingsModel>> settingsBehaviors)
    {
        _settingsBehaviors = settingsBehaviors.ToList();
        ApplySettingsBehaviors(_choreography.Settings);

        this.WhenAnyValue(state => state.Choreography)
            .Skip(1)
            .Subscribe(choreography => ApplySettingsBehaviors(choreography.Settings));
    }

    [Reactive]
    private ChoreographyModel _choreography = new();

    [Reactive]
    private SvgDocument? _svgDocument;

    [Reactive]
    private string? _svgFilePath;

    [ReactiveCollection]
    private ObservableCollectionExtended<SceneViewModel> _scenes = [];

    [Reactive]
    private SceneViewModel? _selectedScene;

    [ReactiveCollection]
    private ObservableCollectionExtended<PositionModel> _selectedPositions = [];

    [Reactive]
    private SelectionRectangle? _selectionRectangle;

    [Reactive]
    private InteractionMode _interactionMode = InteractionMode.View;

    [Reactive]
    private bool _isPlaceMode;

    private void ApplySettingsBehaviors(SettingsModel settings)
    {
        _settingsBehaviorDisposables.Dispose();
        _settingsBehaviorDisposables = new CompositeDisposable();

        foreach (var behavior in _settingsBehaviors)
        {
            behavior.Activate(settings, _settingsBehaviorDisposables);
        }
    }
}
