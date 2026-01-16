namespace MaterialDesignDemo.Maui.Fields;

public static class DependencyInjection
{
    public static IServiceCollection AddFields(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<FieldsViewModel>, FieldsPage>();
        services.AddTransient<FieldsViewModel>();

        return services;
    }
}
