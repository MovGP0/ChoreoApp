using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.DataGrids;

public partial class DataGridsPage
{
    public DataGridsPage(DataGridsViewModel viewModel)
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
