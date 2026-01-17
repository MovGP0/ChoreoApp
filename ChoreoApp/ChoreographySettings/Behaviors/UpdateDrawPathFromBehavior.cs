using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateDrawPathFromBehavior(
    IPublisher<RedrawFloorCommand> redrawFloorPublisher):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.DrawPathFrom = Preferences.Default.Get(SettingsPreferenceKeys.DrawPathFrom, false);

        viewModel
            .WhenAnyValue(vm => vm.DrawPathFrom)
            .Skip(1)
            .Subscribe(value =>
            {
                Preferences.Default.Set(SettingsPreferenceKeys.DrawPathFrom, value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
