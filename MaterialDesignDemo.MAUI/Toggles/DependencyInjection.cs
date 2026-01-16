namespace MaterialDesignDemo.Maui.Toggles;

public static class DependencyInjection
{
    public static IServiceCollection AddToggles(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<TogglesViewModel>, TogglesPage>();
        services.AddTransient<TogglesPage>();
        services.AddTransient<TogglesViewModel>();
        return services;
    }
}
