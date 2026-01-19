using ChoreoApp.Settings.Behaviors;

namespace ChoreoApp.Settings;

public static class DependencyInjection
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<SettingsViewModel>, SettingsPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<IBehavior<SettingsViewModel>, LoadSettingsPreferencesBehavior>();
        services.AddTransient<IBehavior<SettingsViewModel>, SwitchDarkLightModeBehavior>();
        services.AddTransient<IBehavior<SettingsViewModel>, ColorPreferencesBehavior>();

        return services;
    }
}
