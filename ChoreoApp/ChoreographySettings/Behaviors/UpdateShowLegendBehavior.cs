using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateShowLegendBehavior(
    IPreferences preferences,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher) :
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
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
