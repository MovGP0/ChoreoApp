namespace ChoreoApp.Scenes;

public static class DependencyInjection
{
    public static IServiceCollection AddScenes(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ScenesPaneViewModel>, ScenesPaneView>();
        services.AddTransient<ScenesPaneView>();
        services.AddTransient<ScenesPaneViewModel>();
        services.AddTransient<IBehavior<ScenesPaneViewModel>, Behaviors.OpenChoreoBehavior>();

        return services;
    }
}
