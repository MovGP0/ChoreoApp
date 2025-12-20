using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class LoadChoreographySettingsBehavior(GlobalStateModel globalState)
    : IBehavior<ChoreographySettingsViewModel>
{
    private static readonly ChoreographySettingsMapper Mapper = new();

    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        globalState
            .WhenAnyValue(gs => gs.Choreography)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(choreography =>
            {
                if (choreography is null)
                {
                    ResetViewModel(viewModel);
                    return;
                }

                Mapper.Map(choreography, viewModel);
            })
            .DisposeWith(disposables);
    }

    private static void ResetViewModel(ChoreographySettingsViewModel viewModel)
    {
        viewModel.Comment = string.Empty;
        viewModel.Name = string.Empty;
        viewModel.Subtitle = string.Empty;
        viewModel.Variation = string.Empty;
        viewModel.Author = string.Empty;
        viewModel.Description = string.Empty;
        viewModel.Date = DateTime.Today;
        viewModel.FloorFront = 0;
        viewModel.FloorBack = 0;
        viewModel.FloorLeft = 0;
        viewModel.FloorRight = 0;
    }
}
