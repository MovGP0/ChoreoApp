using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SelectedIconBehavior(
    ILogger<DancerSettingsViewModel> logger):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(SelectedIconBehavior), nameof(DancerSettingsViewModel));
        viewModel
            .WhenAnyValue(vm => vm.SelectedIconOption)
            .Subscribe(option =>
            {
                if (viewModel.SelectedDancer is null)
                {
                    return;
                }

                viewModel.SelectedDancer.Icon = option is null
                    ? null
                    : NormalizeIconName(option.Path);
            })
            .DisposeWith(disposables);
    }

    private static string NormalizeIconName(string iconPath)
    {
        var normalized = iconPath.Replace('\\', '/');
        var fileName = Path.GetFileName(normalized);
        var name = Path.GetFileNameWithoutExtension(fileName);
        return string.IsNullOrWhiteSpace(name)
            ? iconPath
            : name;
    }
}
