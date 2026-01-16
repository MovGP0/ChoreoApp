using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.ColorTool;

public partial class ColorToolPage
{
    public ColorToolPage(ColorToolViewModel viewModel)
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
