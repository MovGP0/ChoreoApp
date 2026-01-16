namespace MaterialDesignDemo.Maui.Trees;

public static class DependencyInjection
{
    public static IServiceCollection AddTrees(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<TreesViewModel>, TreesPage>();
        services.AddTransient<TreesPage>();
        services.AddTransient<TreesViewModel>();
        return services;
    }
}
