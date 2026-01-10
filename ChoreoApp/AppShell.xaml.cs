using ChoreoApp.Dancers;
using ChoreoApp.Settings;

namespace ChoreoApp;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(DancerSettingsPage), typeof(DancerSettingsPage));
    }
}
