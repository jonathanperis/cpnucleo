namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Ef Core
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        // Dapper
        services.AddScoped(_ => new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));

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

        var fakeDataRequested = configuration.GetValue<bool>("CreateFakeData");

        if (!fakeDataRequested) return;
        
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        var logger = loggerFactory.CreateLogger<Type>();
        
        logger.LogInformation("Fake data creation requested. Starting to create fake data.");
        FakeData.Init();    
        logger.LogInformation("Finished creating fake data. Please move the generated 003-database-dump-dml.sql file in the root WebApi project folder to the docker-entrypoint-initdb.d folder.");    
    }
    
    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseDelta(
            getConnection: httpContext => httpContext.RequestServices.GetRequiredService<NpgsqlConnection>());
    }
}
