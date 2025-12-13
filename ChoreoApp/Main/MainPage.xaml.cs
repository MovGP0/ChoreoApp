using Microsoft.Maui.Controls.Foldable;

namespace ChoreoApp.Main;

public partial class MainPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
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
