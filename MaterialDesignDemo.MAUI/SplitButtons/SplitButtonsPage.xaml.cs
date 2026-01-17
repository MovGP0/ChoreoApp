using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.SplitButtons;

public partial class SplitButtonsPage
{
    public SplitButtonsPage(SplitButtonsViewModel viewModel)
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
