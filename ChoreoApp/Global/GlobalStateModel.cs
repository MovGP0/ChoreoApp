using System.Collections.ObjectModel;
using ChoreoApp.Scenes;
using ChoreoMasterMobile.Json;

namespace ChoreoApp.Global;

public sealed partial class GlobalStateModel : ReactiveObject
{
    [Reactive]
    private Choreography? _choreography;

    [ReactiveCollection]
    private ObservableCollection<SceneViewModel> _scenes = [];

    [Reactive]
    private SceneViewModel? _selectedScene;
}
