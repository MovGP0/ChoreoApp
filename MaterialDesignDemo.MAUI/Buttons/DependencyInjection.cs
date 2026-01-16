namespace MaterialDesignDemo.Maui.Buttons;

public static class DependencyInjection
{
    public static IServiceCollection AddButtons(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ButtonsViewModel>, ButtonsPage>();
        services.AddTransient<ButtonsViewModel>();

        return services;
    }
}
