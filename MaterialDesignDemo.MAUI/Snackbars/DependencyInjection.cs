namespace MaterialDesignDemo.Maui.Snackbars;

public static class DependencyInjection
{
    public static IServiceCollection AddSnackbars(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<SnackbarsViewModel>, SnackbarsPage>();
        services.AddTransient<SnackbarsPage>();
        services.AddTransient<SnackbarsViewModel>();
        return services;
    }
}
