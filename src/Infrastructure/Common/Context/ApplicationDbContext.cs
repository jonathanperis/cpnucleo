namespace Infrastructure.Common.Context;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IConfiguration? _configuration;

    public DbSet<Impediment>? Impediments { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<Domain.Entities.System>? Systems { get; set; }
    public DbSet<Domain.Entities.Task>? Tasks { get; set; }
    public DbSet<TaskImpediment>? TaskImpediments { get; set; }
    public DbSet<TaskType>? TaskTypes { get; set; }
    public DbSet<TimeKeep>? TimeKeeps { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserProject>? UserProjects { get; set; }
    public DbSet<UserTask>? UserTasks { get; set; }
    public DbSet<Workflow>? Workflows { get; set; }

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Other configurations
        modelBuilder.ApplyConfiguration(new ImpedimentMap());
        modelBuilder.ApplyConfiguration(new ProjectMap());
        modelBuilder.ApplyConfiguration(new SystemMap());
        modelBuilder.ApplyConfiguration(new TaskMap());
        modelBuilder.ApplyConfiguration(new TaskImpedimentMap());
        modelBuilder.ApplyConfiguration(new TaskTypeMap());
        modelBuilder.ApplyConfiguration(new TimeKeepMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new UserProjectMap());
        modelBuilder.ApplyConfiguration(new UserTaskMap());
        modelBuilder.ApplyConfiguration(new WorkflowMap());

        CreateSeedData();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(_configuration?.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToBytesConverter>();
    }

    public async new Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken) > 0;
    }

    private static void CreateSeedData()
    {

    }
}
