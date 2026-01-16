namespace MaterialDesignDemo.MAUI.ToolTips;

public static class DependencyInjection
{
    public static IServiceCollection AddToolTips(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ToolTipsViewModel>, ToolTipsPage>();
        services.AddTransient<ToolTipsPage>();
        services.AddTransient<ToolTipsViewModel>();
        return services;
    }
}
