using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateDrawPathToBehavior(
    IPublisher<RedrawFloorCommand> redrawFloorPublisher):
    IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.DrawPathTo = Preferences.Default.Get(SettingsPreferenceKeys.DrawPathTo, false);

        viewModel
            .WhenAnyValue(vm => vm.DrawPathTo)
            .Skip(1)
            .Subscribe(value =>
            {
                Preferences.Default.Set(SettingsPreferenceKeys.DrawPathTo, value);
                redrawFloorPublisher.Publish(new());
            })
            .DisposeWith(disposables);
    }
}
