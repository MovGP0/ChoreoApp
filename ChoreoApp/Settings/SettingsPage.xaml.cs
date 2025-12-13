using ReactiveUI.Maui;

namespace ChoreoApp.Settings;

public partial class SettingsPage : ReactiveContentPage<SettingsViewModel>
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }
}
