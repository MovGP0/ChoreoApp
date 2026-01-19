using System.Reactive.Disposables;
using ChoreoApp.Models;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.ChoreographySettings.Behaviors;

public sealed class LoadSettingsPreferencesBehavior(IPreferences preferences, ILogger<SettingsModel> logger) : IBehavior<SettingsModel>
{
    public void Activate(SettingsModel settings, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(LoadSettingsPreferencesBehavior), nameof(SettingsModel));
        settings.ShowTimestamps = preferences.Get(SettingsPreferenceKeys.ShowTimestamps, true);
        settings.PositionsAtSide = preferences.Get(SettingsPreferenceKeys.PositionsAtSide, true);
        settings.SnapToGrid = preferences.Get(SettingsPreferenceKeys.SnapToGrid, true);
    }
}
