using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateGridResolutionBehavior(GlobalStateModel globalState)
    : IBehavior<ChoreographySettingsViewModel>
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

                choreography.Settings ??= new();
                choreography.Settings.Resolution = Math.Clamp(option.Value, 1, 16);
            })
            .DisposeWith(disposables);
    }
}
