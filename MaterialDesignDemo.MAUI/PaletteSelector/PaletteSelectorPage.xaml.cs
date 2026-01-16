using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.PaletteSelector;

public partial class PaletteSelectorPage
{
    public PaletteSelectorPage(PaletteSelectorViewModel viewModel)
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
