namespace ChoreoApp.Scenes;

public static class DependencyInjection
{
    public static IServiceCollection AddScenes(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ScenesPaneViewModel>, ScenesPaneView>();
        services.AddTransient<ScenesPaneView>();
        services.AddTransient<ScenesPaneViewModel>();

        return services;
    }
}
