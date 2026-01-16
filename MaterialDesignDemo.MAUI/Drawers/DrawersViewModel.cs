using System.Collections.ObjectModel;
using MaterialDesignThemes.Maui;

namespace MaterialDesignDemo.Maui.Drawers;

public sealed partial class DrawersViewModel : ReactiveObject, IActivatableViewModel
{
    public DrawersViewModel()
    {
        OpenModes = new ObservableCollection<DrawerHostOpenMode>
        {
            DrawerHostOpenMode.Default,
            DrawerHostOpenMode.Modal,
            DrawerHostOpenMode.Standard
        };
        OpenMode = DrawerHostOpenMode.Default;
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<DrawerHostOpenMode> OpenModes { get; }

    [Reactive]
    private DrawerHostOpenMode _openMode;

    [Reactive]
    private bool _isPrimaryOverlay;

    [Reactive]
    private bool _isLeftDrawerOpen;

    [Reactive]
    private bool _isTopDrawerOpen;

    [Reactive]
    private bool _isRightDrawerOpen;

    [Reactive]
    private bool _isBottomDrawerOpen;

    [ReactiveCommand]
    private Task OpenLeftAsync()
    {
        IsLeftDrawerOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenTopAsync()
    {
        IsTopDrawerOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenRightAsync()
    {
        IsRightDrawerOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenBottomAsync()
    {
        IsBottomDrawerOpen = true;
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task CloseAllAsync()
    {
        IsLeftDrawerOpen = false;
        IsTopDrawerOpen = false;
        IsRightDrawerOpen = false;
        IsBottomDrawerOpen = false;
        return Task.CompletedTask;
    }
}
