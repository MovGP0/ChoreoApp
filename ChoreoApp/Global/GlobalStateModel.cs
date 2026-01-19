using ChoreoApp.Models;
using ChoreoApp.Scenes;
using ChoreoApp.Settings;
using DynamicData.Binding;

namespace ChoreoApp.Global;

public sealed partial class GlobalStateModel : ReactiveObject, StateMachine.IGlobalStateModel
{
    private readonly SettingsViewModel _settingsViewModel;
    private readonly IDisposable _settingsActivation;

    public GlobalStateModel(SettingsViewModel settingsViewModel)
    {
        _settingsViewModel = settingsViewModel;
        _settingsActivation = _settingsViewModel.Activator.Activate();
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
}
