using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.ChoreographySettings;

public partial class ChoreographySettingsView
{
    public ChoreographySettingsView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel?.Activator.Activate().DisposeWith(disposables);
        });
    }
}
