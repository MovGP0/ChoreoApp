using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes;

public partial class DeleteSceneDialogView
{
    public DeleteSceneDialogView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel?.Activator.Activate().DisposeWith(disposables);
        });
    }
}
