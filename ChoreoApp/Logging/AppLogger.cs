using Microsoft.Extensions.Logging;

namespace ChoreoApp.Logging;

public static class AppLogger
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
                field ??= LoggerFactory.Create(builder => builder.AddDebug());
            }

            return field;
        }
    }

    public static ILogger CreateLogger<T>() => Factory.CreateLogger<T>();

    public static ILogger CreateLogger(string categoryName) => Factory.CreateLogger(categoryName);
}
