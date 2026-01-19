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

public sealed class UpdatePositionsAtSideBehavior(
    GlobalStateModel globalState,
    IPreferences preferences,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ILogger<ChoreographySettingsViewModel> logger):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(UpdatePositionsAtSideBehavior), nameof(ChoreographySettingsViewModel));
        globalState.Choreography.Settings.PositionsAtSide = preferences.Get(SettingsPreferenceKeys.PositionsAtSide, true);

        viewModel
            .WhenAnyValue(vm => vm.PositionsAtSide)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Settings.PositionsAtSide = value;
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
