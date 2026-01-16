namespace MaterialDesignDemo.Maui.Dialogs;

public static class DependencyInjection
{
    public static IServiceCollection AddDialogs(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<DialogsViewModel>, DialogsPage>();
        services.AddTransient<DialogsViewModel>();

        return services;
    }
}
