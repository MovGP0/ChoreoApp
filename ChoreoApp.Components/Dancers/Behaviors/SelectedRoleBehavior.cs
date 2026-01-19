using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SelectedRoleBehavior(
    ILogger<DancerSettingsViewModel> logger) :
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(SelectedRoleBehavior), nameof(DancerSettingsViewModel));
        viewModel
            .WhenAnyValue(vm => vm.SelectedRole)
            .Subscribe(role =>
            {
                if (viewModel.SelectedDancer is null || role is null)
                {
                    return;
                }

                viewModel.SelectedDancer.Role = role;
            })
            .DisposeWith(disposables);
    }
}
