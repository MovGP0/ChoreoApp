namespace MaterialDesignDemo.Maui.Drawers;

public static class DependencyInjection
{
    public static IServiceCollection AddDrawers(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<DrawersViewModel>, DrawersPage>();
        services.AddTransient<DrawersViewModel>();

        return services;
    }
}
