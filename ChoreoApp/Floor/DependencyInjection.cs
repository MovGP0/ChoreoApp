namespace ChoreoApp.Floor;

public static class DependencyInjection
{
    public static IServiceCollection AddFloor(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<FloorCanvasViewModel>, FloorCanvasView>();
        services.AddTransient<FloorCanvasViewModel>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.DrawFloorBehavior>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.RedrawFloorBehavior>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.GestureHandlingBehavior>();

        return services;
    }
}
