using Microsoft.Extensions.DependencyInjection;
using ChoreoApp.Scenes;
using ChoreoApp.Styling;

namespace ChoreoApp.Main;

public partial class MainPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;

        var scenesVm = MauiProgram.Services.GetRequiredService<ScenesPaneViewModel>();
        LeftDrawerScenes.ViewModel = scenesVm;
        LeftDrawerScenes.BindingContext = scenesVm;

        Drawer.DrawerOpened += OnDrawerOpened;
        Drawer.DrawerClosing += OnDrawerClosing;
    }

    private void OnDrawerOpened(object? sender, DrawerOpenedEventArgs e)
    {
        if (e.Dock == DrawerDock.Left && !HamburgerButton.IsChecked)
        {
            HamburgerButton.IsChecked = true;
        }
    }

    private void OnDrawerClosing(object? sender, DrawerClosingEventArgs e)
    {
        if (e.Dock == DrawerDock.Left && HamburgerButton.IsChecked)
        {
            HamburgerButton.IsChecked = false;
        }
    }

    private void OnBurgerClicked(object sender, EventArgs e)
    {
        Drawer.IsLeftDrawerOpen = HamburgerButton.IsChecked;
    }
}
