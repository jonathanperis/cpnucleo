namespace Infrastructure.Common.Context;

internal sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public DbSet<Impediment>? Impediments { get; }
    public DbSet<Project>? Projects { get; }
    public DbSet<Domain.System>? Systems { get; }
    public DbSet<Domain.Task>? Tasks { get; }
    public DbSet<TaskImpediment>? TaskImpediments { get; }
    public DbSet<TaskType>? TaskTypes { get; }
    public DbSet<TimeKeep>? TimeKeeps { get; }
    public DbSet<User>? Users { get; }
    public DbSet<UserProject>? UserProjects { get; }
    public DbSet<UserTask>? UserTasks { get; }
    public DbSet<Workflow>? Workflows { get; }

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

    private static void CreateSeedData()
    {

    }    
}