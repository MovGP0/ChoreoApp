using ChoreoMasterMobile.Json;

namespace ChoreoApp;

public sealed partial class GlobalStateModel : ReactiveObject
{
    [Reactive]
    private Choreography? _choreography;
}
