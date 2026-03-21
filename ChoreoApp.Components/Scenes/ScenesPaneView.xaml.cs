using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes;

public partial class ScenesPaneView
{
    public ScenesPaneView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel?.Activator.Activate().DisposeWith(disposables);
        });
    }
}
