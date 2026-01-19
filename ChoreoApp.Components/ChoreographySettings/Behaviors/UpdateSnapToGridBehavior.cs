using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Models;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateSnapToGridBehavior(
    GlobalStateModel globalState,
    IPreferences preferences,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ILogger<ChoreographySettingsViewModel> logger):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(UpdateSnapToGridBehavior), nameof(ChoreographySettingsViewModel));
        var snapToGrid = preferences.Get(SettingsPreferenceKeys.SnapToGrid, true);
        viewModel.SnapToGrid = snapToGrid;

        if (globalState.Choreography is { } choreography)
        {
            choreography.Settings.SnapToGrid = snapToGrid;
        }

        viewModel
            .WhenAnyValue(vm => vm.SnapToGrid)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Settings.SnapToGrid = value;
                preferences.Set(SettingsPreferenceKeys.SnapToGrid, value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
