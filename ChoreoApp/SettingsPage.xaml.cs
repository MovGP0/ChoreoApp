namespace ChoreoApp;

using ReactiveUI.Maui;

public partial class SettingsPage : ReactiveContentPage<SettingsViewModel>
{
    public SettingsPage()
    {
        InitializeComponent();
        ViewModel ??= new SettingsViewModel();
    }
}
