using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SelectedDancerStateBehavior(
    ILogger<DancerSettingsViewModel> logger) :
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(SelectedDancerStateBehavior), nameof(DancerSettingsViewModel));
        viewModel
            .WhenAnyValue(vm => vm.SelectedDancer)
            .Subscribe(dancer =>
            {
                viewModel.HasSelectedDancer = dancer is not null;
                viewModel.CanDeleteDancer = dancer is not null;
                viewModel.SelectedIconOption = dancer is null
                    ? null
                    : viewModel.IconOptions.FirstOrDefault(option => IsIconMatch(option, dancer.Icon));
                viewModel.SelectedRole = dancer?.Role;
            })
            .DisposeWith(disposables);
    }

    private static bool IsIconMatch(IconOption option, string? iconValue)
    {
        if (string.IsNullOrWhiteSpace(iconValue))
        {
            return false;
        }

        if (string.Equals(option.Key, iconValue, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var normalizedIcon = iconValue.Replace('\\', '/');
        var normalizedPath = option.Path.Replace('\\', '/');
        if (string.Equals(normalizedPath, normalizedIcon, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var iconFile = Path.GetFileName(normalizedIcon);
        var optionFile = Path.GetFileName(normalizedPath);
        return string.Equals(iconFile, optionFile, StringComparison.OrdinalIgnoreCase);
    }
}
