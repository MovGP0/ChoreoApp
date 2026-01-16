using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Buttons;

public partial class ButtonsPage
{
    public ButtonsPage(ButtonsViewModel viewModel)
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
