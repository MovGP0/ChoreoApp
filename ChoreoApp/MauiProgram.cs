using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ChoreoApp.AudioPlayer;
using ChoreoApp.ChoreographySettings;
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

        builder.Services.AddSingleton<Global.GlobalStateModel>();

        builder.Services
            .AddFloor()
            .AddAudio()
            .AddScenes()
            .AddChoreographySettings()
            .AddSettings()
            .AddMain();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        Services = app.Services;
        return app;
    }

    public static IServiceProvider Services { get; private set; } = null!;
}
