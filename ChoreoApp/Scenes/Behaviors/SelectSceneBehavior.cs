using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes.Behaviors;

public sealed class SelectSceneBehavior: IBehavior<ScenesPaneViewModel>
{
    public void Activate(ScenesPaneViewModel viewModel, CompositeDisposable disposables)
    {
        SceneViewModel? previous = null;

        viewModel
            .WhenAnyValue(vm => vm.SelectedScene)
            .Subscribe(current =>
            {
                if (ReferenceEquals(previous, current))
                {
                    return;
                }

                if (previous is not null)
                {
                    previous.IsSelected = false;
                }

                if (current is not null)
                {
                    current.IsSelected = true;
                }

                previous = current;
            })
            .DisposeWith(disposables);
    }
}


