using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes;

public partial class CopyScenePositionsDialogView
{
    public CopyScenePositionsDialogView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel?.Activator.Activate().DisposeWith(disposables);
        });
    }
}
