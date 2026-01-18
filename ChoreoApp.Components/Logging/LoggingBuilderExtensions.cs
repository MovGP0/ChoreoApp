using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ChoreoApp.Logging;

public static class LoggingBuilderExtensions
{
    [Conditional("DEBUG")]
    public static void ConfigureLogging(this ILoggingBuilder builder)
    {
        builder.AddDebug();
    }
}
