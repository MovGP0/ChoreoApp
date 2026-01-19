using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using MessagePipe;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateShowLegendBehavior(
    IPreferences preferences,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ILogger<ChoreographySettingsViewModel> logger) :
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(UpdateShowLegendBehavior), nameof(ChoreographySettingsViewModel));
        viewModel.ShowLegend = preferences.Get(SettingsPreferenceKeys.ShowLegend, false);

        viewModel
            .WhenAnyValue(vm => vm.ShowLegend)
            .Skip(1)
            .Subscribe(value =>
            {
                preferences.Set(SettingsPreferenceKeys.ShowLegend, value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
