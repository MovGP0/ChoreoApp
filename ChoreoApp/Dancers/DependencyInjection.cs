namespace ChoreoApp.Dancers;

public static class DependencyInjection
{
    public static IServiceCollection AddDancers(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<DancerSettingsViewModel>, DancerSettingsPage>();
        services.AddTransient<DancerSettingsPage>();
        services.AddTransient<DancerSettingsViewModel>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.LoadDancerSettingsBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.SelectedDancerStateBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.SelectedIconBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.SelectedRoleBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.AddDancerBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.DeleteDancerBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.SaveDancerSettingsBehavior>();
        services.AddTransient<IBehavior<DancerSettingsViewModel>, Behaviors.CancelDancerSettingsBehavior>();

        return services;
    }
}
