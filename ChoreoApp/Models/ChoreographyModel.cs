using DynamicData.Binding;

namespace ChoreoApp.Models;

public sealed partial class ChoreographyModel : ReactiveObject
{
    [Reactive]
    private string? _comment;

    [Reactive]
    private SettingsModel _settings = new();

    [Reactive]
    private FloorModel _floor = new();

    [ReactiveCollection]
    private ObservableCollectionExtended<RoleModel> _roles = [];

    [ReactiveCollection]
    private ObservableCollectionExtended<DancerModel> _dancers = [];

    [ReactiveCollection]
    private ObservableCollectionExtended<SceneModel> _scenes = [];

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private string? _subtitle;

    [Reactive]
    private string? _date;

    [Reactive]
    private string? _variation;

    [Reactive]
    private string? _author;

    [Reactive]
    private string? _description;

    [Reactive]
    private DateTimeOffset _lastSaveDate = DateTimeOffset.UtcNow;
}
