using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateFloorColorBehavior(GlobalStateModel globalState)
    : IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.FloorColor)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Settings ??= new();
                choreography.Settings.FloorColor = value;
            })
            .DisposeWith(disposables);
    }
}
