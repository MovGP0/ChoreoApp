using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.ChoreographySettings.Messages;
using ChoreoApp.Global;
using MessagePipe;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class ShowSceneTimestampsBehavior(
    GlobalStateModel globalState,
    ISubscriber<ShowTimestampsChangedEvent> showTimestampsChangedSubscriber):
    IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
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
            viewModel.ShowTimestamps = globalState.Choreography?.Settings?.ShowTimestamps ?? false;
        }
    }
}
