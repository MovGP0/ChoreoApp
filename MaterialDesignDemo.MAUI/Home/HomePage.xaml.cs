using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Home;

public partial class HomePage
{
    public HomePage(HomeViewModel viewModel)
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
