namespace MaterialDesignDemo.Maui.ComboBoxes;

public static class DependencyInjection
{
    public static IServiceCollection AddComboBoxes(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ComboBoxesViewModel>, ComboBoxesPage>();
        services.AddTransient<ComboBoxesViewModel>();

        return services;
    }
}
