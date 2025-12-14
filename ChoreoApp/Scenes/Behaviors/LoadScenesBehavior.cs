using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Linq;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class LoadScenesBehavior(
    GlobalStateModel globalState) :
    IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        // when a new choreography is loaded, refresh the scenes list
        globalState
            .WhenAnyValue(gs => gs.Choreography)
            .Subscribe(choreography =>
            {
                if (choreography is null)
                {
                    viewModel.Scenes.Clear();
                    viewModel.SelectedScene = null;
                    return;
                }

                var scenes = choreography.Scenes
                    .OrderBy(e => e.Timestamp);

                viewModel.Scenes.Clear();
                foreach (var scene in scenes)
                {
                    var sceneVm = new SceneViewModel(scene.Name, scene.Color);
                    viewModel.Scenes.Add(sceneVm);
                }

                viewModel.SelectedScene = viewModel.Scenes.FirstOrDefault();
            })
            .DisposeWith(disposables);
    }
}


