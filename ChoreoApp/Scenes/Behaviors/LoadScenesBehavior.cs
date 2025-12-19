using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class LoadScenesBehavior(
    GlobalStateModel globalState,
    IServiceProvider serviceProvider) :
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
                    ClearScenes();
                    viewModel.SelectedScene = null;
                    return;
                }

                var scenes = choreography.Scenes
                    .OrderBy(e => e.Timestamp);

                ClearScenes();
                foreach (var scene in scenes)
                {
                    var sceneVm = serviceProvider.GetRequiredService<SceneViewModel>();
                    sceneVm.Name = scene.Name;
                    sceneVm.Color = scene.Color;
                    sceneVm.Activator.Activate();
                    viewModel.Scenes.Add(sceneVm);
                }

                viewModel.SelectedScene = viewModel.Scenes.FirstOrDefault();
            })
            .DisposeWith(disposables);

        void ClearScenes()
        {
            foreach (var sceneVm in viewModel.Scenes)
            {
                sceneVm.Activator.Deactivate();
            }

            viewModel.Scenes.Clear();
        }
    }
}
