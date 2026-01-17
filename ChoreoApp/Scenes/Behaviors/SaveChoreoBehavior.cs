using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Models;
using ChoreoMasterMobile.Json;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class SaveChoreoBehavior(Global.GlobalStateModel globalState, IPreferences preferences) : IBehavior<ScenesPaneViewModel>
{
    private static readonly ChoreographyModelMapper Mapper = new();
    private static readonly SceneMapper SceneMapper = new();

    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .SaveChoreoCommand
            .Subscribe(async _ => await HandleSaveAsync())
            .DisposeWith(disposables);
    }

    private Task HandleSaveAsync()
    {
        if (globalState.Choreography is not { } choreography)
        {
            return Task.CompletedTask;
        }

        var path = preferences.Get(SettingsPreferenceKeys.LastOpenedChoreoFile, string.Empty);
        if (string.IsNullOrWhiteSpace(path))
        {
            return Task.CompletedTask;
        }

        SyncSceneModels(globalState, choreography);
        choreography.LastSaveDate = DateTimeOffset.UtcNow;
        var jsonModel = Mapper.Map(choreography);
        Util.ExportToFile(path, jsonModel);
        return Task.CompletedTask;
    }

    private static void SyncSceneModels(Global.GlobalStateModel globalState, ChoreographyModel choreography)
    {
        var scenes = new List<SceneModel>(globalState.Scenes.Count);
        foreach (var sceneViewModel in globalState.Scenes)
        {
            var existingScene = choreography.Scenes.FirstOrDefault(scene => scene.SceneId == sceneViewModel.SceneId)
                ?? choreography.Scenes.FirstOrDefault(scene => string.Equals(scene.Name, sceneViewModel.Name, StringComparison.Ordinal))
                ?? new SceneModel();

            SceneMapper.Map(sceneViewModel, existingScene);
            scenes.Add(existingScene);
        }

        choreography.Scenes.Clear();
        foreach (var scene in scenes)
        {
            choreography.Scenes.Add(scene);
        }
    }
}
