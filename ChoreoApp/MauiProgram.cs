using SkiaSharp.Views.Maui.Controls.Hosting;
using ChoreoApp.AudioPlayer;
using ChoreoApp.ChoreographySettings;
using ChoreoApp.Dancers;
using ChoreoApp.Floor;
using ChoreoApp.Main;
using ChoreoApp.Scenes;
using ChoreoApp.Settings;
using ChoreoApp.StateMachine;
using CommunityToolkit.Maui;
using Sharpnado.Shades;

namespace ChoreoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("DroidSansMNerdFont-Regular.otf", "DroidSans");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.UseSharpnadoShadows(enableLogging: false);

        //! returns the MessagePipeBuilder which we do not need here
        _ = builder.Services.AddMessagePipe();

        builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System);
        builder.Services.AddSingleton<Global.GlobalStateModel>();

        builder.Services
            .AddApplicationStateMachine()
            .AddFloor()
            .AddAudio()
            .AddScenes()
            .AddChoreographySettings()
            .AddDancers()
            .AddSettings()
            .AddMain();

        builder.Logging.ConfigureLogging();

        var app = builder.Build();
        Services = app.Services;
        return app;
    }

    public static IServiceProvider Services { get; private set; } = null!;
}
