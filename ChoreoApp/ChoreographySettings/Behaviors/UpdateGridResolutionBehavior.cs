using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateGridResolutionBehavior(
    GlobalStateModel globalState,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.SelectedGridSizeOption)
            .Skip(1)
            .Subscribe(option =>
            {
                if (option is null || globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Settings.Resolution = Math.Clamp(option.Value, 1, 16);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
