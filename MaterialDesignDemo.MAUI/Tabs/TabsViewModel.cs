using System.Collections.ObjectModel;

namespace MaterialDesignDemo.Maui.Tabs;

public sealed partial class TabsViewModel : ReactiveObject, IActivatableViewModel
{
    public TabsViewModel()
    {
        DefaultTabs =
        [
            new TabItem("TAB 1", "Default Tab 1"),
            new TabItem("TAB 2", "Default Tab 2"),
            new TabItem("TAB 3", "Default Tab 3")
        ];

        FilledTabs =
        [
            new TabItem("TAB 1", "Filled Tab 1"),
            new TabItem("TAB 2", "Filled Tab 2")
        ];

        SelectedDefaultTab = DefaultTabs[0];
        SelectedFilledTab = FilledTabs[0];
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<TabItem> DefaultTabs { get; }
    public ObservableCollection<TabItem> FilledTabs { get; }

    [Reactive]
    private TabItem? _selectedDefaultTab;

    [Reactive]
    private TabItem? _selectedFilledTab;
}
