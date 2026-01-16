namespace MaterialDesignDemo.Maui.ColorZones;

public static class DependencyInjection
{
    public static IServiceCollection AddColorZones(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ColorZonesViewModel>, ColorZonesPage>();
        services.AddTransient<ColorZonesViewModel>();

        return services;
    }
}
