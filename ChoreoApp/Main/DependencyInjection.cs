namespace ChoreoApp.Main;

public static class DependencyInjection
{
    public static IServiceCollection AddMain(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<MainViewModel>, MainPage>();
        services.AddTransient<MainPage>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<IBehavior<MainViewModel>, Behaviors.OpenSvgFileBehavior>();
        services.AddTransient<IBehavior<MainViewModel>, Behaviors.ShowDialogBehavior>();
        services.AddTransient<IBehavior<MainViewModel>, Behaviors.HideDialogBehavior>();
        return services;
    }
}
