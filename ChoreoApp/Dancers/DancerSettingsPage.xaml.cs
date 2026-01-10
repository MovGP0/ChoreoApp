using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Dancers;

public partial class DancerSettingsPage
{
    public DancerSettingsPage(DancerSettingsViewModel viewModel)
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
