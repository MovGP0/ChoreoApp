using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class SelectedIconBehavior : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
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
