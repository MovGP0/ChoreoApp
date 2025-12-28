using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateAuthorBehavior(GlobalStateModel globalState)
    : IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel
            .WhenAnyValue(vm => vm.Author)
            .Skip(1)
            .Subscribe(value =>
            {
                if (globalState.Choreography is not { } choreography)
                {
                    return;
                }

                choreography.Author = NormalizeText(value);
            })
            .DisposeWith(disposables);
    }

    private static string? NormalizeText(string value)
        => string.IsNullOrWhiteSpace(value) ? null : value;
}
