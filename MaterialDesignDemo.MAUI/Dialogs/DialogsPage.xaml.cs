using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Dialogs;

public partial class DialogsPage
{
    public DialogsPage(DialogsViewModel viewModel)
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
