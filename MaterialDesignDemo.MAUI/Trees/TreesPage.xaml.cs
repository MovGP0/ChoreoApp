using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Trees;

public partial class TreesPage
{
    public TreesPage(TreesViewModel viewModel)
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
