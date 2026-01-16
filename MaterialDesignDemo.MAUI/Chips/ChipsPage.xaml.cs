using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Chips;

public partial class ChipsPage
{
    public ChipsPage(ChipsViewModel viewModel)
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
