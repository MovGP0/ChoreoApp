namespace MaterialDesignDemo.Maui.SplitButtons;

public static class DependencyInjection
{
    public static IServiceCollection AddSplitButtons(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<SplitButtonsViewModel>, SplitButtonsPage>();
        services.AddTransient<SplitButtonsViewModel>();

        return services;
    }
}
