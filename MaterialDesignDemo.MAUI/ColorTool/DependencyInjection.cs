namespace MaterialDesignDemo.Maui.ColorTool;

public static class DependencyInjection
{
    public static IServiceCollection AddColorTool(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ColorToolViewModel>, ColorToolPage>();
        services.AddTransient<ColorToolViewModel>();

        return services;
    }
}
