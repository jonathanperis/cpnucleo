namespace Infrastructure.Common.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IConfiguration? _configuration;

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

        var seedDataRequested = _configuration!.GetValue<bool>("SEED_DATA");

        if (!seedDataRequested) return;
        
        FakeData.Init();

        modelBuilder.Entity<Impediment>().HasData(FakeData.Impediments!);
        modelBuilder.Entity<Project>().HasData(FakeData.Projects!);
        modelBuilder.Entity<Organization>().HasData(FakeData.Organizations!);
        modelBuilder.Entity<Assignment>().HasData(FakeData.Assignments!);
        modelBuilder.Entity<AssignmentImpediment>().HasData(FakeData.AssignmentImpediments!);
        modelBuilder.Entity<AssignmentType>().HasData(FakeData.AssignmentTypes!);
        modelBuilder.Entity<Appointment>().HasData(FakeData.Appointments!);
        modelBuilder.Entity<User>().HasData(FakeData.Users!);
        modelBuilder.Entity<UserProject>().HasData(FakeData.UserProjects!);
        modelBuilder.Entity<UserAssignment>().HasData(FakeData.UserAssignments!);
        modelBuilder.Entity<Workflow>().HasData(FakeData.Workflows!);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(_configuration?.GetConnectionString("DefaultConnection"));     

            // optionsBuilder
            //     .UseSqlServer(_configuration?.GetConnectionString("DefaultConnection"));
        }
    }

    public new async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken) > 0;
    }
}
