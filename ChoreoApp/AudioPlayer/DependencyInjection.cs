using ChoreoApp.AudioPlayer.Behaviors;
using Plugin.Maui.Audio;

namespace ChoreoApp.AudioPlayer;

public static class DependencyInjection
{
    public static IServiceCollection AddAudio(this IServiceCollection services)
    {
        services.AddSingleton<IAudioManager>(_ => AudioManager.Current);

        services.AddTransient<IViewFor<AudioPlayerViewModel>, AudioPlayerView>();
        services.AddTransient<AudioPlayerView>();
        services.AddTransient<AudioPlayerViewModel>();
        services.AddTransient<AudioPlayerPage>();
        services.AddTransient<IBehavior<AudioPlayerViewModel>, AudioPlayerBehavior>();
        services.AddTransient<IBehavior<AudioPlayerViewModel>, OpenAudioFileBehavior>();
        services.AddTransient<IBehavior<AudioPlayerViewModel>, CloseAudioFileBehavior>();
        services.AddTransient<IBehavior<AudioPlayerViewModel>, AudioPlayerTicksBehavior>();
        services.AddTransient<IBehavior<AudioPlayerViewModel>, AudioPlayerLinkSceneBehavior>();
        services.AddTransient<IBehavior<AudioPlayerViewModel>, AudioPlayerPositionChangedBehavior>();

        return services;
    }
}
