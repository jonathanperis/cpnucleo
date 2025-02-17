namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Dapper.SqlMapper.AddTypeHandler(typeof(Guid), new BinaryGuidHandler());
        // Dapper.SqlMapper.AddTypeHandler(typeof(Guid), new StringGuidHandler());

        // Ef Core
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        // Dapper
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentImpedimentRepository, AssignmentImpedimentRepository>();
        services.AddScoped<IAssignmentTypeRepository, AssignmentTypeRepository>();
        services.AddScoped<IImpedimentRepository, ImpedimentRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAssignmentRepository, UserAssignmentRepository>();
        services.AddScoped<IUserProjectRepository, UserProjectRepository>();
        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
    }
}
