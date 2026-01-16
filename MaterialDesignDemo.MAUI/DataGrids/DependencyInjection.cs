namespace MaterialDesignDemo.Maui.DataGrids;

public static class DependencyInjection
{
    public static IServiceCollection AddDataGrids(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<DataGridsViewModel>, DataGridsPage>();
        services.AddTransient<DataGridsViewModel>();

        return services;
    }
}
