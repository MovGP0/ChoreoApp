using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;
using ChoreoApp.Models;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SaveDancerSettingsBehavior(GlobalStateModel globalState)
    : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.SaveCommand
            .SelectMany(_ => Observable.FromAsync(() => SaveAsync(viewModel)))
            .Subscribe()
            .DisposeWith(disposables);
    }

    private async Task SaveAsync(DancerSettingsViewModel viewModel)
    {
        var choreography = globalState.Choreography;
        if (choreography is null)
        {
            await NavigateBackAsync();
            return;
        }

        ApplyChangesToChoreography(viewModel, choreography);
        await NavigateBackAsync();
    }

    private static async Task NavigateBackAsync()
    {
        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync("..");
        }
    }

    private static void ApplyChangesToChoreography(DancerSettingsViewModel viewModel, ChoreographyModel choreography)
    {
        choreography.Roles.Clear();
        foreach (var role in viewModel.Roles)
        {
            choreography.Roles.Add(role);
        }

        choreography.Dancers.Clear();
        foreach (var dancer in viewModel.Dancers)
        {
            choreography.Dancers.Add(dancer);
        }

        var dancerMap = viewModel.Dancers.ToDictionary(dancer => dancer.DancerId, dancer => dancer);
        foreach (var scene in EnumerateScenes(choreography))
        {
            for (int index = scene.Positions.Count - 1; index >= 0; index--)
            {
                var position = scene.Positions[index];
                if (position.Dancer is null)
                {
                    continue;
                }

                var dancerId = position.Dancer.DancerId;
                if (!dancerMap.TryGetValue(dancerId, out var newDancer))
                {
                    scene.Positions.RemoveAt(index);
                    continue;
                }

                position.Dancer = newDancer;
            }
        }
    }

    private static IEnumerable<SceneModel> EnumerateScenes(ChoreographyModel choreography)
    {
        var visited = new HashSet<SceneModel>();
        foreach (var scene in choreography.Scenes)
        {
            foreach (var item in EnumerateScenes(scene, visited))
            {
                yield return item;
            }
        }
    }

    private static IEnumerable<SceneModel> EnumerateScenes(SceneModel scene, HashSet<SceneModel> visited)
    {
        if (!visited.Add(scene))
        {
            yield break;
        }

        yield return scene;

        foreach (var variation in scene.Variations)
        {
            foreach (var variationScene in variation)
            {
                foreach (var item in EnumerateScenes(variationScene, visited))
                {
                    yield return item;
                }
            }
        }

        foreach (var variationScene in scene.CurrentVariation)
        {
            foreach (var item in EnumerateScenes(variationScene, visited))
            {
                yield return item;
            }
        }
    }
}