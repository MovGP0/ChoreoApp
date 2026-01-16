using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Transitions;

public partial class TransitionsPage
{
    public TransitionsPage(TransitionsDemoViewModel viewModel)
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
