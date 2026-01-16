namespace MaterialDesignDemo.Maui.Home;

public static class DependencyInjection
{
    public static IServiceCollection AddHome(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<HomeViewModel>, HomePage>();
        services.AddTransient<HomePage>();
        services.AddTransient<HomeViewModel>();
        return services;
    }
}
