namespace ChoreoApp.Floor;

public static class DependencyInjection
{
    public static IServiceCollection AddFloor(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<FloorCanvasViewModel>, FloorCanvasView>();
        services.AddTransient<FloorCanvasViewModel>();

        return services;
    }
}
