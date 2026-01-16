using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Elevation;

public partial class ElevationPage
{
    public ElevationPage(ElevationViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;

        this.WhenActivated(disposables =>
        {
            viewModel.Activator.Activate().DisposeWith(disposables);
        });
    }
}
