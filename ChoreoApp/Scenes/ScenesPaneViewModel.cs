using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChoreoApp;
using ChoreoApp.Settings;

namespace ChoreoApp.Scenes;

public sealed partial class ScenesPaneViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _searchText = string.Empty;

    [ReactiveCollection]
    private ObservableCollection<NavItemViewModel> _navItems =
    [
        new("Choreo A"),
        new("Choreo B"),
        new("Choreo C"),
        new("Choreo D"),
        new("Choreo E")
    ];

    public void MoveNavItem(NavItemViewModel? item, NavItemViewModel? target)
    {
        if (item is null || target is null || item == target)
        {
            return;
        }

        var oldIndex = NavItems.IndexOf(item);
        var newIndex = NavItems.IndexOf(target);

        if (oldIndex < 0 || newIndex < 0 || oldIndex == newIndex)
        {
            return;
        }

        NavItems.RemoveAt(oldIndex);
        NavItems.Insert(newIndex, item);
    }

    [Reactive]
    private bool _canNavigateToSettings = true;

    [ReactiveCommand(CanExecute = nameof(CanNavigateToSettings))]
    private async Task NavigateToSettingsAsync()
    {
        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync(nameof(SettingsPage));
        }
    }
}
