using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Drawers;

public partial class DrawersPage
{
    public DrawersPage(DrawersViewModel viewModel)
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
