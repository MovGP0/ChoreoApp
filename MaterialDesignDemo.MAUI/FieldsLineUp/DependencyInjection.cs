namespace MaterialDesignDemo.Maui.FieldsLineUp;

public static class DependencyInjection
{
    public static IServiceCollection AddFieldsLineUp(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<FieldsLineUpViewModel>, FieldsLineUpPage>();
        services.AddTransient<FieldsLineUpViewModel>();

        return services;
    }
}
