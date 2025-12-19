using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoMasterMobile.Json;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class LoadScenesBehavior(
    Global.GlobalStateModel globalState,
    IServiceProvider serviceProvider) :
    IBehavior<ScenesPaneViewModel>
{
    private static readonly SceneMapper Mapper = new();

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

                SceneId nextSceneId = scenes
                    .Select(scene => scene.SceneId)
                    .DefaultIfEmpty(SceneId.Empty)
                    .Max();

                ClearScenes();
                foreach (Scene scene in scenes)
                {
                    if (scene.SceneId.Value <= 0)
                    {
                        nextSceneId = new(nextSceneId.Value + 1);
                        scene.SceneId = nextSceneId;
                    }
                    else
                    {
                        nextSceneId = new(Math.Max(nextSceneId.Value, scene.SceneId.Value));
                    }

                    var sceneVm = serviceProvider.GetRequiredService<SceneViewModel>();
                    Mapper.Map(scene, sceneVm);
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
