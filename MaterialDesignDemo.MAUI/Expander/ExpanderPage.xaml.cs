using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Expander;

public partial class ExpanderPage
{
    public ExpanderPage(ExpanderViewModel viewModel)
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
