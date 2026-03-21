using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class LoadChoreographySettingsBehavior(GlobalStateModel globalState, ILogger<ChoreographySettingsViewModel> logger)
    : IBehavior<ChoreographySettingsViewModel>
{
    private static readonly ChoreographySettingsMapper Mapper = new();

    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(LoadChoreographySettingsBehavior), nameof(ChoreographySettingsViewModel));
        globalState
            .WhenAnyValue(gs => gs.Choreography)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(choreography =>
            {
                if (choreography is null)
                {
                    ResetViewModel(viewModel);
                    return;
                }

                Mapper.Map(choreography, viewModel);
            })
            .DisposeWith(disposables);
    }

    private static void ResetViewModel(ChoreographySettingsViewModel viewModel)
    {
        viewModel.Comment = string.Empty;
        viewModel.Name = string.Empty;
        viewModel.Subtitle = string.Empty;
        viewModel.Variation = string.Empty;
        viewModel.Author = string.Empty;
        viewModel.Description = string.Empty;
        viewModel.Date = DateTime.Today;
        viewModel.FloorFront = 0;
        viewModel.FloorBack = 0;
        viewModel.FloorLeft = 0;
        viewModel.FloorRight = 0;
        viewModel.GridResolution = 1;
        viewModel.Transparency = 0m;
        viewModel.PositionsAtSide = false;
        viewModel.GridLines = false;
        viewModel.SnapToGrid = true;
        viewModel.FloorColor = Colors.Transparent;
        viewModel.ShowTimestamps = false;
        viewModel.HasSelectedScene = false;
        viewModel.SceneName = string.Empty;
        viewModel.SceneText = string.Empty;
        viewModel.SceneFixedPositions = false;
        viewModel.SceneHasTimestamp = false;
        viewModel.SceneTimestamp = TimeSpan.Zero;
        viewModel.SceneColor = Colors.Transparent;
    }
}
