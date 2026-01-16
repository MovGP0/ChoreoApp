using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.MAUI.ToolTips;

public partial class ToolTipsPage
{
    public ToolTipsPage(ToolTipsViewModel viewModel)
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
