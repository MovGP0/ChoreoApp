using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateSelectedSceneBehavior(GlobalStateModel globalState, ILogger<ChoreographySettingsViewModel> logger)
    : IBehavior<ChoreographySettingsViewModel>
{
    private bool _isUpdating;

    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(UpdateSelectedSceneBehavior), nameof(ChoreographySettingsViewModel));
        globalState
            .WhenAnyValue(gs => gs.SelectedScene)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(scene =>
            {
                _isUpdating = true;
                MapSceneToViewModel(viewModel, scene);
                _isUpdating = false;
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SceneName)
            .Skip(1)
            .Where(_ => !_isUpdating)
            .Subscribe(name =>
            {
                if (globalState.SelectedScene is { } scene)
                {
                    scene.Name = name;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SceneText)
            .Skip(1)
            .Where(_ => !_isUpdating)
            .Subscribe(text =>
            {
                if (globalState.SelectedScene is { } scene)
                {
                    scene.Text = text;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SceneFixedPositions)
            .Skip(1)
            .Where(_ => !_isUpdating)
            .Subscribe(value =>
            {
                if (globalState.SelectedScene is { } scene)
                {
                    scene.FixedPositions = value;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SceneColor)
            .Skip(1)
            .Where(_ => !_isUpdating)
            .Subscribe(color =>
            {
                if (globalState.SelectedScene is { } scene)
                {
                    scene.Color = color;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.SceneHasTimestamp, vm => vm.SceneTimestamp)
            .Skip(1)
            .Where(_ => !_isUpdating)
            .Subscribe(tuple =>
            {
                if (globalState.SelectedScene is not { } scene)
                {
                    return;
                }

                if (!tuple.Item1)
                {
                    scene.Timestamp = null;
                    return;
                }

                scene.Timestamp = tuple.Item2;
            })
            .DisposeWith(disposables);
    }

    private static void MapSceneToViewModel(ChoreographySettingsViewModel viewModel, Scenes.SceneViewModel? scene)
    {
        if (scene is null)
        {
            viewModel.HasSelectedScene = false;
            viewModel.SceneName = string.Empty;
            viewModel.SceneText = string.Empty;
            viewModel.SceneFixedPositions = false;
            viewModel.SceneHasTimestamp = false;
            viewModel.SceneTimestamp = TimeSpan.Zero;
            viewModel.SceneColor = Colors.Transparent;
            return;
        }

        viewModel.HasSelectedScene = true;
        viewModel.SceneName = scene.Name;
        viewModel.SceneText = scene.Text ?? string.Empty;
        viewModel.SceneFixedPositions = scene.FixedPositions;
        viewModel.SceneHasTimestamp = scene.Timestamp.HasValue;
        viewModel.SceneTimestamp = scene.Timestamp ?? TimeSpan.Zero;
        viewModel.SceneColor = scene.Color;
    }
}
