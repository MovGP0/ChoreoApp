using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.i18n;

namespace ChoreoApp.Main.Behaviors;

public sealed class UpdateChoreographyTitleBehavior(
    GlobalStateModel globalState):
    IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        globalState
            .WhenAnyValue(e => e.Choreography)
            .Subscribe(choreography =>
            {
                if (choreography is null)
                {
                    viewModel.Title = Translations.AppTitle;
                    return;
                }

                viewModel.Title = choreography.Name;
            })
            .DisposeWith(disposables);
    }
}
