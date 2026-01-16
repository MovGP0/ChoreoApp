namespace MaterialDesignDemo.Maui.DocumentationLinks;

public static class DependencyInjection
{
    public static IServiceCollection AddDocumentationLinks(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<DocumentationLinksViewModel>, DocumentationLinksPage>();
        services.AddTransient<DocumentationLinksViewModel>();

        return services;
    }
}
