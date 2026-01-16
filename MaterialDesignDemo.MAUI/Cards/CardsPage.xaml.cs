using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.Cards;

public partial class CardsPage
{
    public CardsPage(CardsViewModel viewModel)
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
