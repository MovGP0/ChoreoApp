using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ChoreoApp.Floor;
using ChoreoApp.Main;
using ChoreoApp.Scenes;
using ChoreoApp.Settings;

namespace ChoreoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("DroidSansMNerdFont-Regular.otf", "DroidSans");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        //! returns the MessagePipeBuilder which we do not need here
        _ = builder.Services.AddMessagePipe();

        builder.Services
            .AddFloor()
            .AddScenes()
            .AddSettings()
            .AddMain();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
