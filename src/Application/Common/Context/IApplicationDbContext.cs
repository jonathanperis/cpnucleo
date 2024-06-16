namespace Application.Common.Context;

public interface IApplicationDbContext
{
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

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
