using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Typography;

public partial class TypographyPage
{
    public TypographyPage(TypographyViewModel viewModel)
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
