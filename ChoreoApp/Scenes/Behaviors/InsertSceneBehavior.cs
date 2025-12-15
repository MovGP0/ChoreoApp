using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoMasterMobile.Json;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class InsertSceneBehavior(
    GlobalStateModel globalState) :
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
        var selectedScene = viewModel.SelectedScene;
        var insertIndex = CalculateInsertIndex(viewModel, insertAfter);
        var name = BuildSceneName(viewModel);
        var color = selectedScene?.Color ?? Colors.Transparent;
        var newSceneViewModel = new SceneViewModel(name, color);

        viewModel.Scenes.Insert(insertIndex, newSceneViewModel);
        viewModel.SelectedScene = newSceneViewModel;

        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        InsertModelScene(choreography, newSceneViewModel, selectedScene, insertIndex);
    }

    private static int CalculateInsertIndex(ScenesPaneViewModel viewModel, bool insertAfter)
    {
        if (viewModel.SelectedScene is null)
        {
            return viewModel.Scenes.Count;
        }

        int selectedIndex = viewModel.Scenes.IndexOf(viewModel.SelectedScene);
        if (selectedIndex < 0)
        {
            return viewModel.Scenes.Count;
        }

        return insertAfter
            ? selectedIndex + 1
            : selectedIndex;
    }

    private static string BuildSceneName(ScenesPaneViewModel viewModel)
    {
        const string baseName = "New Scene";

        if (!viewModel.Scenes.Any(scene => string.Equals(scene.Name, baseName, StringComparison.Ordinal)))
        {
            return baseName;
        }

        int suffix = 2;
        while (viewModel.Scenes.Any(scene => string.Equals($"{baseName} {suffix}", scene.Name, StringComparison.Ordinal)))
        {
            suffix++;
        }

        return $"{baseName} {suffix}";
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
            : scenes.FirstOrDefault(scene => string.Equals(scene.Name, selectedScene.Name, StringComparison.Ordinal));

        var timestamp = selectedModelScene?.Timestamp;
        var color = selectedModelScene?.Color ?? newSceneViewModel.Color;

        scenes.Insert(insertIndex, new Scene
        {
            Name = newSceneViewModel.Name,
            Color = color,
            Timestamp = timestamp
        });
    }
}
