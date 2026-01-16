namespace MaterialDesignDemo.Maui.Expander;

public static class DependencyInjection
{
    public static IServiceCollection AddExpander(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ExpanderViewModel>, ExpanderPage>();
        services.AddTransient<ExpanderViewModel>();

        return services;
    }
}
