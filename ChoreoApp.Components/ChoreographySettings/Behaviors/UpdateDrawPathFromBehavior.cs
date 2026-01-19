using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateDrawPathFromBehavior(
    IPreferences preferences,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ILogger<ChoreographySettingsViewModel> logger):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(UpdateDrawPathFromBehavior), nameof(ChoreographySettingsViewModel));
        viewModel.DrawPathFrom = preferences.Get(SettingsPreferenceKeys.DrawPathFrom, false);

        viewModel
            .WhenAnyValue(vm => vm.DrawPathFrom)
            .Skip(1)
            .Subscribe(value =>
            {
                preferences.Set(SettingsPreferenceKeys.DrawPathFrom, value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
