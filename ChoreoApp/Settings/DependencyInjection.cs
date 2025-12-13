using ChoreoApp.Settings.Behaviors;

namespace ChoreoApp.Settings;

public static class DependencyInjection
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<SettingsViewModel>, SettingsPage>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<IBehavior<SettingsViewModel>, SwitchDarkLightModeBehavior>();

        return services;
    }
}
