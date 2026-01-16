using System.Collections.ObjectModel;

namespace MaterialDesignDemo.Maui.ComboBoxes;

public sealed partial class ComboBoxesViewModel : ReactiveObject, IActivatableViewModel
{
    public ComboBoxesViewModel()
    {
        Items = new ObservableCollection<string>
        {
            "Mercury",
            "Venus",
            "Earth",
            "Mars",
            "Jupiter"
        };
        SelectedItem = Items[2];
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<string> Items { get; }

    [Reactive]
    private string? _selectedItem;
}
