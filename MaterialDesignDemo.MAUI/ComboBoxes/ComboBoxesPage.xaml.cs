using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.ComboBoxes;

public partial class ComboBoxesPage
{
    public ComboBoxesPage(ComboBoxesViewModel viewModel)
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
