namespace MaterialDesignDemo.MAUI.Transitions;

public static class DependencyInjection
{
    public static IServiceCollection AddTransitions(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<TransitionsDemoViewModel>, TransitionsPage>();
        services.AddTransient<TransitionsDemoHomeView>();
        services.AddTransient<TransitionsPage>();
        services.AddTransient<TransitionsDemoViewModel>();
        return services;
    }
}
