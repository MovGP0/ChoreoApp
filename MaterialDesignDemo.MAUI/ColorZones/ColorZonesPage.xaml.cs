using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.ColorZones;

public partial class ColorZonesPage
{
    public ColorZonesPage(ColorZonesViewModel viewModel)
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
