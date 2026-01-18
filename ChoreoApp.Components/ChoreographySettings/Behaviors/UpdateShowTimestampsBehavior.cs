using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.ChoreographySettings.Messages;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateShowTimestampsBehavior(
    GlobalStateModel globalState,
    IPreferences preferences,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    IPublisher<ShowTimestampsChangedEvent> showTimestampsChangedPublisher):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        globalState.Choreography.Settings.ShowTimestamps = preferences.Get(SettingsPreferenceKeys.ShowTimestamps, true);

        viewModel
            .WhenAnyValue(vm => vm.ShowTimestamps)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Settings.ShowTimestamps = value;
                redrawFloorPublisher.Publish(new());
                showTimestampsChangedPublisher.Publish(new(value));
            })
            .DisposeWith(disposables);
    }
}
