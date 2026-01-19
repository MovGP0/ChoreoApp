using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.ChoreographySettings.Messages;
using ChoreoApp.Global;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class ShowSceneTimestampsBehavior(
    GlobalStateModel globalState,
    ISubscriber<ShowTimestampsChangedEvent> showTimestampsChangedSubscriber,
    ILogger<ScenesPaneViewModel> logger):
    IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(ShowSceneTimestampsBehavior), nameof(ScenesPaneViewModel));
        UpdateFromChoreography();

        globalState
            .WhenAnyValue(gs => gs.Choreography)
            .Subscribe(_ => UpdateFromChoreography())
            .DisposeWith(disposables);

        showTimestampsChangedSubscriber
            .Subscribe(evnt => viewModel.ShowTimestamps = evnt.IsEnabled)
            .DisposeWith(disposables);

        void UpdateFromChoreography()
        {
            viewModel.ShowTimestamps = globalState.Choreography.Settings.ShowTimestamps;
        }
    }
}
