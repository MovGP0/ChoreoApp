using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Snackbars;

public partial class SnackbarsPage
{
    public SnackbarsPage(SnackbarsViewModel viewModel)
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
