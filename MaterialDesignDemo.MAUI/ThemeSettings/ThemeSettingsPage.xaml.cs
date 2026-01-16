using System.Reactive.Disposables.Fluent;

namespace MaterialDesignDemo.Maui.ThemeSettings;

public partial class ThemeSettingsPage
{
    public ThemeSettingsPage(ThemeSettingsViewModel viewModel)
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
