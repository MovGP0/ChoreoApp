using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class CancelDancerSettingsBehavior(
    ILogger<DancerSettingsViewModel> logger) :
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(CancelDancerSettingsBehavior), nameof(DancerSettingsViewModel));
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
