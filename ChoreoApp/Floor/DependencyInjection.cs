namespace ChoreoApp.Floor;

public static class DependencyInjection
{
    public static IServiceCollection AddFloor(this IServiceCollection services)
    {
        services.AddSingleton<IFloorRenderGate, FloorRenderGate>();
        services.AddTransient<IViewFor<FloorCanvasViewModel>, FloorCanvasView>();
        services.AddTransient<FloorCanvasViewModel>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.DrawFloorBehavior>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.RedrawFloorBehavior>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.GestureHandlingBehavior>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.PlacePositionBehavior>();
        services.AddTransient<IBehavior<FloorCanvasViewModel>, Behaviors.MovePositionsBehavior>();

        return services;
    }
}
