using Microsoft.Extensions.Logging;

namespace ChoreoApp.Logging;

internal static partial class BehaviorLog
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Behavior activated: {BehaviorName} for {ViewModelName}.")]
    public static partial void BehaviorActivated(ILogger logger, string behaviorName, string viewModelName);
}
