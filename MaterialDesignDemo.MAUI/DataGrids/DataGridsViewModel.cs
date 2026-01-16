using System.Collections.ObjectModel;

namespace MaterialDesignDemo.Maui.DataGrids;

public sealed partial class DataGridsViewModel : ReactiveObject, IActivatableViewModel
{
    public DataGridsViewModel()
    {
        Items = new ObservableCollection<DataGridRow>
        {
            new("12345", "Mercury", "Active", 12),
            new("23456", "Venus", "Active", 8),
            new("34567", "Earth", "Pending", 15),
            new("45678", "Mars", "Active", 5),
            new("56789", "Jupiter", "Paused", 22)
        };
        SelectedItem = Items[2];
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<DataGridRow> Items { get; }

    [Reactive]
    private DataGridRow? _selectedItem;
}

public sealed record DataGridRow(string Id, string Name, string Status, int Count);
