using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.DocumentationLinks;

public partial class DocumentationLinksPage
{
    public DocumentationLinksPage(DocumentationLinksViewModel viewModel)
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
