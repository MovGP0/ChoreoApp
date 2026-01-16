using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.FieldsLineUp;

public partial class FieldsLineUpPage
{
    public FieldsLineUpPage(FieldsLineUpViewModel viewModel)
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
