namespace MaterialDesignDemo.Maui.Elevation;

public static class DependencyInjection
{
    public static IServiceCollection AddElevation(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ElevationViewModel>, ElevationPage>();
        services.AddTransient<ElevationViewModel>();

        return services;
    }
}
