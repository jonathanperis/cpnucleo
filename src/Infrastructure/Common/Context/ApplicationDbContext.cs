namespace Infrastructure.Common.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Impediment>? Impediments { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<Organization>? Organizations { get; set; }
    public DbSet<Assignment>? Assignments { get; set; }
    public DbSet<AssignmentImpediment>? AssignmentImpediments { get; set; }
    public DbSet<AssignmentType>? AssignmentTypes { get; set; }
    public DbSet<Appointment>? Appointments { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserProject>? UserProjects { get; set; }
    public DbSet<UserAssignment>? UserAssignments { get; set; }
    public DbSet<Workflow>? Workflows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ImpedimentMap());
        modelBuilder.ApplyConfiguration(new ProjectMap());
        modelBuilder.ApplyConfiguration(new OrganizationMap());
        modelBuilder.ApplyConfiguration(new AssignmentMap());
        modelBuilder.ApplyConfiguration(new AssignmentImpedimentMap());
        modelBuilder.ApplyConfiguration(new AssignmentTypeMap());
        modelBuilder.ApplyConfiguration(new AppointmentMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new UserProjectMap());
        modelBuilder.ApplyConfiguration(new UserAssignmentMap());
        modelBuilder.ApplyConfiguration(new WorkflowMap());

        modelBuilder.Entity<Impediment>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<Project>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<Organization>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<Assignment>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<AssignmentImpediment>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<AssignmentType>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<Appointment>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<User>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<UserProject>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<UserAssignment>().HasQueryFilter(x => x.Active);
        modelBuilder.Entity<Workflow>().HasQueryFilter(x => x.Active);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(_configuration.GetValue<string>("DB_CONNECTION_STRING"));
        }
    }

    public new async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken) > 0;
    }
}
