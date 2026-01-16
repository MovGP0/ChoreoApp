using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Toggles;

public partial class TogglesPage
{
    public TogglesPage(TogglesViewModel viewModel)
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
