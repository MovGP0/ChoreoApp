namespace ChoreoApp.ChoreographySettings;

public static class DependencyInjection
{
    public static IServiceCollection AddChoreographySettings(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ChoreographySettingsViewModel>, ChoreographySettingsView>();
        services.AddTransient<ChoreographySettingsView>();
        services.AddTransient<ChoreographySettingsViewModel>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.LoadChoreographySettingsBehavior>();
        return services;
    }
}
