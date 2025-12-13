using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Settings;

public partial class SettingsPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;

        this.WhenActivated(disposables =>
        {
            viewModel.Activator.Activate()
                .DisposeWith(disposables);
        });
    }
}
