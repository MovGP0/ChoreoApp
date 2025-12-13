namespace ChoreoApp.Main;

public static class DependencyInjection
{
    public static IServiceCollection AddMain(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<MainViewModel>, MainPage>();
        services.AddTransient<MainViewModel>();

        return services;
    }
}
