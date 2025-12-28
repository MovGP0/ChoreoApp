namespace ChoreoApp.StateMachine;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationStateMachine(this IServiceCollection services)
    {
        return services
            .AddSingleton<ApplicationStateMachine>()
            // TODO: register transitions here...
            ;
    }
}
