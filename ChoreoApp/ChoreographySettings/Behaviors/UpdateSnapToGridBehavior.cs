using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateSnapToGridBehavior(
    GlobalStateModel globalState,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        var snapToGrid = Preferences.Default.Get(SettingsPreferenceKeys.SnapToGrid, true);
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
                Preferences.Default.Set(SettingsPreferenceKeys.SnapToGrid, value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
