using System.Reactive.Disposables;
using ChoreoApp.Models;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class LoadSettingsPreferencesBehavior(IPreferences preferences) : IBehavior<SettingsModel>
{
    public void Activate(SettingsModel settings, CompositeDisposable disposables)
    {
        settings.ShowTimestamps = preferences.Get(SettingsPreferenceKeys.ShowTimestamps, true);
        settings.PositionsAtSide = preferences.Get(SettingsPreferenceKeys.PositionsAtSide, true);
        settings.SnapToGrid = preferences.Get(SettingsPreferenceKeys.SnapToGrid, true);
    }
}
