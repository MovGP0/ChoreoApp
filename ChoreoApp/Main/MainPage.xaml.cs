using ChoreoApp.Floor;
using ChoreoApp.Scenes;
using Microsoft.Maui.Controls.Foldable;

namespace ChoreoApp.Main;

public partial class MainPage
{
    private readonly ScenesPaneViewModel _scenesViewModel = new();
    private readonly FloorCanvasViewModel _floorViewModel = new();

    public MainPage()
    {
        InitializeComponent();
        ViewModel ??= new MainViewModel();

        SinglePaneScenes.BindingContext = _scenesViewModel;
        DualPaneScenes.BindingContext = _scenesViewModel;
        SinglePaneFloor.BindingContext = _floorViewModel;
        DualPaneFloor.BindingContext = _floorViewModel;

        this.WhenActivated(disposables =>
        {
            // Bindings and activation logic can go here.
        });
    }

    private void OnMainPageLoaded(object sender, EventArgs e)
    {
        UpdatePaneLayout(MainTwoPaneView.Mode);
    }

    private void OnTwoPaneModeChanged(object sender, EventArgs e)
    {
        UpdatePaneLayout(MainTwoPaneView.Mode);
    }

    private void UpdatePaneLayout(TwoPaneViewMode mode)
    {
        var isSinglePane = mode == TwoPaneViewMode.SinglePane;

        SinglePaneHost.IsVisible = isSinglePane;
        DualPaneScenesHost.IsVisible = !isSinglePane;

        MainTwoPaneView.PanePriority = TwoPaneViewPriority.Pane1;
    }

    private void OnBurgerClicked(object sender, EventArgs e)
    {
        ViewModel?.ToggleNavigation();
    }
}
