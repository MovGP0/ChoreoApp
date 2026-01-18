using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateFloorLeftBehavior(
    GlobalStateModel globalState,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.FloorLeft)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Floor.SizeLeft = Math.Clamp(value, 0, 100);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
