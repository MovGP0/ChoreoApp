using System.Reactive.Disposables;
using ChoreoApp.Models;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class LoadSettingsPreferencesBehavior : IBehavior<SettingsModel>
{
    public void Activate(SettingsModel settings, CompositeDisposable disposables)
    {
        settings.ShowTimestamps = Preferences.Default.Get(SettingsPreferenceKeys.ShowTimestamps, true);
        settings.PositionsAtSide = Preferences.Default.Get(SettingsPreferenceKeys.PositionsAtSide, true);
        settings.SnapToGrid = Preferences.Default.Get(SettingsPreferenceKeys.SnapToGrid, true);
    }
}
