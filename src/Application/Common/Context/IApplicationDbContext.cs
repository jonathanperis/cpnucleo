namespace Application.Common.Context;

public interface IApplicationDbContext
{
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

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}