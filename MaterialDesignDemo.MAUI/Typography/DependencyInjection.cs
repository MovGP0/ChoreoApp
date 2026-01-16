namespace MaterialDesignDemo.Maui.Typography;

public static class DependencyInjection
{
    public static IServiceCollection AddTypography(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<TypographyViewModel>, TypographyPage>();
        services.AddTransient<TypographyPage>();
        services.AddTransient<TypographyViewModel>();
        return services;
    }
}
