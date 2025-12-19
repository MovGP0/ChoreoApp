using ChoreoApp.Scenes;
using ChoreoMasterMobile.Json;
using DynamicData.Binding;

namespace ChoreoApp.Global;

public sealed partial class GlobalStateModel : ReactiveObject
{
    [Reactive]
    private Choreography? _choreography;

    [ReactiveCollection]
    private ObservableCollectionExtended<SceneViewModel> _scenes = [];

    [Reactive]
    private SceneViewModel? _selectedScene;
}
