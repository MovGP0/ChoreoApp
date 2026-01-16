using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Fields;

public partial class FieldsPage
{
    public FieldsPage(FieldsViewModel viewModel)
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
