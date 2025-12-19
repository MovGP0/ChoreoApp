using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoMasterMobile.Json;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class InsertSceneBehavior(
    Global.GlobalStateModel globalState,
    IServiceProvider serviceProvider) :
    IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .AddSceneBeforeCommand
            .Subscribe(_ => InsertScene(viewModel, insertAfter: false))
            .DisposeWith(disposables);

        viewModel
            .AddSceneAfterCommand
            .Subscribe(_ => InsertScene(viewModel, insertAfter: true))
            .DisposeWith(disposables);
    }

    private void InsertScene(ScenesPaneViewModel viewModel, bool insertAfter)
    {
        var allScenes = globalState.Scenes;
        var selectedScene = globalState.SelectedScene;
        var insertIndex = CalculateInsertIndex(allScenes, selectedScene, insertAfter);
        var name = BuildSceneName(allScenes);
        var color = selectedScene?.Color ?? Colors.Transparent;

        var newSceneViewModel = serviceProvider.GetRequiredService<SceneViewModel>();
        newSceneViewModel.SceneId = GetNextSceneId(allScenes);
        newSceneViewModel.Name = name;
        newSceneViewModel.Color = color;

        allScenes.Insert(insertIndex, newSceneViewModel);
        globalState.SelectedScene = newSceneViewModel;

        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        InsertModelScene(choreography, newSceneViewModel, selectedScene, insertIndex);
    }

    private static int CalculateInsertIndex(
        IList<SceneViewModel> scenes,
        SceneViewModel? selectedScene,
        bool insertAfter)
    {
        if (selectedScene is null)
        {
            return scenes.Count;
        }

        int selectedIndex = scenes.IndexOf(selectedScene);
        if (selectedIndex < 0)
        {
            return scenes.Count;
        }

        return insertAfter
            ? selectedIndex + 1
            : selectedIndex;
    }

    private static string BuildSceneName(IList<SceneViewModel> scenes)
    {
        const string baseName = "New Scene";

        if (!scenes.Any(scene => string.Equals(scene.Name, baseName, StringComparison.Ordinal)))
        {
            return baseName;
        }

        int suffix = 2;
        while (scenes.Any(scene => string.Equals($"{baseName} {suffix}", scene.Name, StringComparison.Ordinal)))
        {
            suffix++;
        }

        return $"{baseName} {suffix}";
    }

    private static int GetNextSceneId(IList<SceneViewModel> scenes)
    {
        if (scenes.Count == 0)
        {
            return 1;
        }

        var maxId = scenes.Max(scene => scene.SceneId);
        return Math.Max(maxId, 0) + 1;
    }

    private static void InsertModelScene(
        Choreography choreography,
        SceneViewModel newSceneViewModel,
        SceneViewModel? selectedScene,
        int viewModelInsertIndex)
    {
        var scenes = choreography.Scenes;
        int insertIndex = Math.Clamp(viewModelInsertIndex, 0, scenes.Count);

        var selectedModelScene = selectedScene is null
            ? null
            : scenes.FirstOrDefault(scene => scene.SceneId == selectedScene.SceneId)
              ?? scenes.FirstOrDefault(scene => string.Equals(scene.Name, selectedScene.Name, StringComparison.Ordinal));

        var timestamp = selectedModelScene?.Timestamp;
        var color = selectedModelScene?.Color ?? newSceneViewModel.Color;

        scenes.Insert(insertIndex, new Scene
        {
            SceneId = newSceneViewModel.SceneId,
            Name = newSceneViewModel.Name,
            Color = color,
            Timestamp = timestamp
        });
    }
}
