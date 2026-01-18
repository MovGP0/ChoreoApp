using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SelectedRoleBehavior : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
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