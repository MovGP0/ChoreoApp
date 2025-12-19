using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Settings;
using ChoreoMasterMobile.Json;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class SaveChoreoBehavior(Global.GlobalStateModel globalState) : IBehavior<ScenesPaneViewModel>
{
    private static readonly SceneMapper Mapper = new();

    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .SaveChoreoCommand
            .Subscribe(async _ => await HandleSaveAsync())
            .DisposeWith(disposables);
    }

    private Task HandleSaveAsync()
    {
        var choreography = globalState.Choreography;

        if (choreography is null)
        {
            return Task.CompletedTask;
        }

        var path = Preferences.Default.Get(SettingsPreferenceKeys.LastOpenedChoreoFile, string.Empty);
        if (string.IsNullOrWhiteSpace(path))
        {
            return Task.CompletedTask;
        }

        var scenes = new List<Scene>(globalState.Scenes.Count);
        foreach (var sceneViewModel in globalState.Scenes)
        {
            var existingScene = choreography.Scenes.FirstOrDefault(scene => scene.SceneId == sceneViewModel.SceneId)
                ?? choreography.Scenes.FirstOrDefault(scene => string.Equals(scene.Name, sceneViewModel.Name, StringComparison.Ordinal))
                ?? new Scene();

            Mapper.Map(sceneViewModel, existingScene);
            scenes.Add(existingScene);
        }

        choreography.Scenes = scenes;
        choreography.LastSaveDate = DateTimeOffset.UtcNow;

        Util.ExportToFile(path, choreography);
        return Task.CompletedTask;
    }
}
