namespace MaterialDesignDemo.Maui.Cards;

public static class DependencyInjection
{
    public static IServiceCollection AddCards(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<CardsViewModel>, CardsPage>();
        services.AddTransient<CardsViewModel>();

        return services;
    }
}
