namespace MaterialDesignDemo.Maui.PaletteSelector;

public static class DependencyInjection
{
    public static IServiceCollection AddPaletteSelector(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<PaletteSelectorViewModel>, PaletteSelectorPage>();
        services.AddTransient<PaletteSelectorViewModel>();

        return services;
    }
}
