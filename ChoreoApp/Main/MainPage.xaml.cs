using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Floor;
using ChoreoApp.Logging;
using ChoreoApp.Scenes;
using Microsoft.Extensions.Logging;
using MaterialDesignThemes.Maui;

namespace ChoreoApp.Main;

public partial class MainPage: IDisposable
{
    private static readonly ILogger Logger = AppLogger.CreateLogger<MainPage>();
    private CompositeDisposable Disposables { get; } = new();
    private readonly IHapticFeedback _hapticFeedback;
    public void Dispose() => Disposables.Dispose();

    public MainPage(
        MainViewModel viewModel,
        ScenesPaneViewModel scenesVm,
        FloorCanvasViewModel floorVm,
        ChoreographySettings.ChoreographySettingsViewModel choreographySettingsViewModel,
        IHapticFeedback hapticFeedback)
    {
        _hapticFeedback = hapticFeedback;
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Logger.LogCritical(ex, "Failed to initialize MainPage XAML.");
            throw;
        }

        // setup bindings
        ViewModel = viewModel;
        BindingContext = viewModel;

        LeftDrawerScenes.ViewModel = scenesVm;
        LeftDrawerScenes.BindingContext = scenesVm;

        MainFloor.ViewModel = floorVm;
        MainFloor.BindingContext = floorVm;

        RightDrawerChoreographySettings.ViewModel = choreographySettingsViewModel;
        RightDrawerChoreographySettings.BindingContext = choreographySettingsViewModel;

        BottomAudioPlayer.ViewModel = viewModel.AudioPlayerViewModel;
        BottomAudioPlayer.BindingContext = viewModel.AudioPlayerViewModel;

        Drawer.DrawerOpened += OnDrawerOpened;
        Disposable.Create(() => Drawer.DrawerOpened -= OnDrawerOpened).DisposeWith(Disposables);

        Drawer.DrawerClosing += OnDrawerClosing;
        Disposable.Create(() => Drawer.DrawerClosing -= OnDrawerClosing).DisposeWith(Disposables);

        this.WhenActivated(disposables =>
        {
            viewModel.Activator.Activate().DisposeWith(disposables);
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
        _hapticFeedback.Perform(HapticFeedbackType.Click);
        Drawer.IsLeftDrawerOpen = HamburgerButton.IsChecked;
    }
}
