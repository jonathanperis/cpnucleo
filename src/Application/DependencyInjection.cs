namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Features.Projects.CreateProject.CreateProjectHandler>();
        return services;
    }
}
