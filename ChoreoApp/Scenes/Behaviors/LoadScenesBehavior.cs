using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class LoadScenesBehavior(
    Global.GlobalStateModel globalState,
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
                    globalState.SelectedScene = null;
                    return;
                }

                var scenes = choreography.Scenes
                    .OrderBy(e => e.Timestamp)
                    .ToList();

                var nextSceneId = scenes
                    .Select(scene => scene.SceneId)
                    .DefaultIfEmpty(0)
                    .Max();

                ClearScenes();
                foreach (var scene in scenes)
                {
                    if (scene.SceneId <= 0)
                    {
                        nextSceneId++;
                        scene.SceneId = nextSceneId;
                    }
                    else
                    {
                        nextSceneId = Math.Max(nextSceneId, scene.SceneId);
                    }

                    var sceneVm = serviceProvider.GetRequiredService<SceneViewModel>();
                    sceneVm.SceneId = scene.SceneId;
                    sceneVm.Name = scene.Name;
                    sceneVm.Color = scene.Color;
                    sceneVm.Activator.Activate();
                    globalState.Scenes.Add(sceneVm);
                }

                globalState.SelectedScene = globalState.Scenes.FirstOrDefault();
            })
            .DisposeWith(disposables);

        void ClearScenes()
        {
            foreach (var sceneVm in globalState.Scenes)
            {
                sceneVm.Activator.Deactivate();
            }

            globalState.Scenes.Clear();
        }
    }
}
