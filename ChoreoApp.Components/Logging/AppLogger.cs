using Microsoft.Extensions.Logging;

namespace ChoreoApp.Logging;

public static partial class AppLogger
{
    private static readonly Lock SyncRoot = new();

    public static ILoggerFactory Factory
    {
        get
        {
            if (field is not null)
            {
                return field;
            }

            lock (SyncRoot)
            {
                return field ??= LoggerFactory.Create(builder => builder.ConfigureLogging());
            }
        }
    }

    public static ILogger CreateLogger<T>() => Factory.CreateLogger<T>();

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Critical,
        Message = "AppDomain unhandled exception. IsTerminating: {IsTerminating}")]
    public static partial void LogUnhandledException(this ILogger logger, Exception exception, bool isTerminating);

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Critical,
        Message = "AppDomain unhandled exception. IsTerminating: {IsTerminating}")]
    public static partial void LogUnhandledException(this ILogger logger, bool isTerminating);

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Critical,
        Message = "Unobserved task exception.")]
    public static partial void LogUnobservedTaskException(this ILogger logger, AggregateException exception);

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Critical,
        Message = "Failed to initialize App resources.")]
    public static partial void LogAppInitializationError(this  ILogger logger, Exception exception);
}
