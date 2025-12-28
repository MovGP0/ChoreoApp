using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Settings;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class UpdateDrawPathToBehavior : IBehavior<ChoreographySettingsViewModel>
{
    public void Activate(ChoreographySettingsViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.DrawPathTo = Preferences.Default.Get(SettingsPreferenceKeys.DrawPathTo, false);

        viewModel
            .WhenAnyValue(vm => vm.DrawPathTo)
            .Skip(1)
            .Subscribe(value =>
            {
                Preferences.Default.Set(SettingsPreferenceKeys.DrawPathTo, value);
            })
            .DisposeWith(disposables);
    }
}
