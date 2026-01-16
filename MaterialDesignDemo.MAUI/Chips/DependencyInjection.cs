namespace MaterialDesignDemo.Maui.Chips;

public static class DependencyInjection
{
    public static IServiceCollection AddChips(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ChipsViewModel>, ChipsPage>();
        services.AddTransient<ChipsViewModel>();

        return services;
    }
}
