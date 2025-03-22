namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        // Dapper Repository Basic
        services.AddScoped(_ => new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IProjectRepository, ProjectRepository>();

        // Dapper Repository Advanced        
        services.AddScoped<IUnitOfWork>(_ => 
            new UnitOfWork(new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))));
        
        var fakeDataRequested = configuration.GetValue<bool>("CreateFakeData");

        if (!fakeDataRequested) return;
        
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        var logger = loggerFactory.CreateLogger<Type>();
        
        logger.LogInformation("Fake data creation requested. Starting to create fake data.");
        FakeDataHelper.CreateSqlCsvDumpFile();    
        logger.LogInformation("Finished creating fake data. Please move the generated dump-dml.sql file and the dml-data in the root WebApi project folder to the docker-entrypoint-initdb.d folder.");    
    }
    
    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseDelta(
            getConnection: httpContext => httpContext.RequestServices.GetRequiredService<NpgsqlConnection>());
    }
}
