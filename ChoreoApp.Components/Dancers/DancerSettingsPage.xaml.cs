using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using MaterialDesignThemes.Maui;

namespace ChoreoApp.Dancers;

public partial class DancerSettingsPage : IDisposable
{
    private CompositeDisposable Disposables { get; } = new();
    public void Dispose() => Disposables.Dispose();

    public DancerSettingsPage(DancerSettingsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;

        Drawer.DrawerOpened += OnDrawerOpened;
        Disposable.Create(() => Drawer.DrawerOpened -= OnDrawerOpened).DisposeWith(Disposables);

        Drawer.DrawerClosing += OnDrawerClosing;
        Disposable.Create(() => Drawer.DrawerClosing -= OnDrawerClosing).DisposeWith(Disposables);

        this.WhenActivated(disposables =>
        {
            viewModel.Activator.Activate()
                .DisposeWith(disposables);
        });
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

    private void OnBurgerClicked(object? sender, EventArgs e)
    {
        Drawer.IsLeftDrawerOpen = HamburgerButton.IsChecked;
    }
}
