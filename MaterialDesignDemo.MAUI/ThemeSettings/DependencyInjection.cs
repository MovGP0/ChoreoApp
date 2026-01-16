namespace MaterialDesignDemo.Maui.ThemeSettings;

public static class DependencyInjection
{
    public static IServiceCollection AddThemeSettings(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ThemeSettingsViewModel>, ThemeSettingsPage>();
        services.AddTransient<ThemeSettingsPage>();
        services.AddTransient<ThemeSettingsViewModel>();
        return services;
    }
}
