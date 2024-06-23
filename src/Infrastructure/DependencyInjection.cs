namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Dapper.SqlMapper.AddTypeHandler(typeof(Ulid), new BinaryUlidHandler());
        // Dapper.SqlMapper.AddTypeHandler(typeof(Ulid), new StringUlidHandler());

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
    }
}
