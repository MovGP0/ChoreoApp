using System.Collections.ObjectModel;

namespace ChoreoApp;

public sealed partial class MainViewModel : ReactiveObject, IActivatableViewModel
{
    private const double DefaultNavWidth = 280d;

    public MainViewModel()
    {
        NavColumnWidth = new GridLength(DefaultNavWidth);
        NavItems = new ObservableCollection<NavItemViewModel>
        {
            new("Choreo A"),
            new("Choreo B"),
            new("Choreo C"),
            new("Choreo D"),
            new("Choreo E")
        };
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    public GridLength NavColumnWidth { get; set; }

    [Reactive]
    public bool IsNavOpen { get; set; } = true;

    [Reactive]
    public string SearchText { get; set; } = string.Empty;

    public ObservableCollection<NavItemViewModel> NavItems { get; }

    public void ToggleNavigation()
    {
        IsNavOpen = !IsNavOpen;
        NavColumnWidth = IsNavOpen ? new GridLength(DefaultNavWidth) : new GridLength(0);
    }

    public void MoveNavItem(NavItemViewModel item, NavItemViewModel target)
    {
        if (item == null || target == null || item == target)
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
}
