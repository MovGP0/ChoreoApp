using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class CancelDancerSettingsBehavior : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.CancelCommand
            .SelectMany(_ => Observable.FromAsync(NavigateBackAsync))
            .Subscribe()
            .DisposeWith(disposables);
    }

    private static async Task NavigateBackAsync()
    {
        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync("..");
        }
    }
}