using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.ChoreographySettings;

public sealed partial class ChoreographySettingsViewModel : ReactiveObject, IActivatableViewModel
{
    public ChoreographySettingsViewModel(IEnumerable<IBehavior<ChoreographySettingsViewModel>> behaviors)
    {
        FloorSizeOptions = Enumerable.Range(0, 101).ToList();

        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    public ViewModelActivator Activator { get; } = new();

    public IReadOnlyList<int> FloorSizeOptions { get; }

    [Reactive]
    private int _floorFront;

    [Reactive]
    private int _floorBack;

    [Reactive]
    private int _floorLeft;

    [Reactive]
    private int _floorRight;

    [Reactive]
    private string _comment = string.Empty;

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private string _subtitle = string.Empty;

    [Reactive]
    private DateTime _date = DateTime.Today;

    [Reactive]
    private string _variation = string.Empty;

    [Reactive]
    private string _author = string.Empty;

    [Reactive]
    private string _description = string.Empty;
}
