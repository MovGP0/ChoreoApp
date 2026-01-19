using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateVariationBehavior(
    GlobalStateModel globalState,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ILogger<ChoreographySettingsViewModel> logger):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(UpdateVariationBehavior), nameof(ChoreographySettingsViewModel));
        viewModel
            .WhenAnyValue(vm => vm.Variation)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Variation = NormalizeText(value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }

    private static string? NormalizeText(string value)
        => string.IsNullOrWhiteSpace(value) ? null : value;
}
