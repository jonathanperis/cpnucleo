using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Unit.Tests.Endpoints;

/// <summary>
/// Every EF Core restore endpoint undoes the soft delete of a whole batch in one SaveChanges, or
/// changes nothing at all.
/// </summary>
public class RestoreEndpointsTests
{
    public sealed record Case(string Name, Func<BaseEntity> Create, Func<IApplicationDbContext, Guid[], Task<(int Status, bool Success)>> Restore)
    {
        public override string ToString() => Name;
    }

    private static readonly Guid AnyId = Guid.NewGuid();

    public static IEnumerable<Case> Cases()
    {
        yield return new("Appointment", () => Appointment.Create("Review", DateTime.UtcNow, 1, AnyId, AnyId),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.Appointment.RestoreAppointment.Endpoint>(db), new WebApi.Endpoints.Appointment.RestoreAppointment.RestoreAppointmentRequest { Ids = [.. ids] }));
        yield return new("Assignment", () => Assignment.Create("Task", "Description", DateTime.UtcNow, DateTime.UtcNow, 1, AnyId, AnyId, AnyId, AnyId),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.Assignment.RestoreAssignment.Endpoint>(db), new WebApi.Endpoints.Assignment.RestoreAssignment.RestoreAssignmentRequest { Ids = [.. ids] }));
        yield return new("AssignmentImpediment", () => AssignmentImpediment.Create("Blocked", AnyId, AnyId),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.AssignmentImpediment.RestoreAssignmentImpediment.Endpoint>(db), new WebApi.Endpoints.AssignmentImpediment.RestoreAssignmentImpediment.RestoreAssignmentImpedimentRequest { Ids = [.. ids] }));
        yield return new("AssignmentType", () => AssignmentType.Create("Exercise"),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.AssignmentType.RestoreAssignmentType.Endpoint>(db), new WebApi.Endpoints.AssignmentType.RestoreAssignmentType.RestoreAssignmentTypeRequest { Ids = [.. ids] }));
        yield return new("Impediment", () => Impediment.Create("Blocked"),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.Impediment.RestoreImpediment.Endpoint>(db), new WebApi.Endpoints.Impediment.RestoreImpediment.RestoreImpedimentRequest { Ids = [.. ids] }));
        yield return new("User", () => User.Create("Learner", $"learner-{Guid.NewGuid():N}", new PasswordHash("hash", "")),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.User.RestoreUser.Endpoint>(db), new WebApi.Endpoints.User.RestoreUser.RestoreUserRequest { Ids = [.. ids] }));
        yield return new("UserAssignment", () => UserAssignment.Create(AnyId, AnyId),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.UserAssignment.RestoreUserAssignment.Endpoint>(db), new WebApi.Endpoints.UserAssignment.RestoreUserAssignment.RestoreUserAssignmentRequest { Ids = [.. ids] }));
        yield return new("UserProject", () => UserProject.Create(AnyId, AnyId),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.UserProject.RestoreUserProject.Endpoint>(db), new WebApi.Endpoints.UserProject.RestoreUserProject.RestoreUserProjectRequest { Ids = [.. ids] }));
        yield return new("Workflow", () => Workflow.Create("Planned", 1),
            (db, ids) => Run(TestEndpoints.Create<WebApi.Endpoints.Workflow.RestoreWorkflow.Endpoint>(db), new WebApi.Endpoints.Workflow.RestoreWorkflow.RestoreWorkflowRequest { Ids = [.. ids] }));
    }

    [TestCaseSource(nameof(Cases))]
    public async Task Restore_ReactivatesEveryRowInTheBatch(Case @case)
    {
        await using var db = CreateDbContext();
        var first = await AddRemovedAsync(db, @case);
        var second = await AddRemovedAsync(db, @case);

        var (status, success) = await @case.Restore(db, [first.Id, second.Id, first.Id]);

        status.ShouldBe(200);
        success.ShouldBeTrue();
        db.ChangeTracker.Clear();
        var rows = await db.Set<BaseEntity>(first.GetType()).IgnoreQueryFilters().AsNoTracking().ToListAsync();
        rows.Count.ShouldBe(2);
        rows.ShouldAllBe(row => row.Active && row.DeletedAt == null && row.UpdatedAt == null, "restore clears the soft delete and changes nothing else");
    }

    [TestCaseSource(nameof(Cases))]
    public async Task Restore_ChangesNothingWhenAnyIdIsMissing(Case @case)
    {
        await using var db = CreateDbContext();
        var removed = await AddRemovedAsync(db, @case);

        (await @case.Restore(db, [removed.Id, Guid.NewGuid()])).Status.ShouldBe(404);

        await ShouldStillBeRemovedAsync(db, removed);
    }

    [TestCaseSource(nameof(Cases))]
    public async Task Restore_ChangesNothingWhenAnyIdIsStillActive(Case @case)
    {
        await using var db = CreateDbContext();
        var removed = await AddRemovedAsync(db, @case);
        var active = @case.Create();
        db.Add(active);
        await db.SaveChangesAsync(default);

        (await @case.Restore(db, [removed.Id, active.Id])).Status.ShouldBe(404, "only removed rows can be restored");

        await ShouldStillBeRemovedAsync(db, removed);
    }

    private static async Task ShouldStillBeRemovedAsync(ApplicationDbContext db, BaseEntity removed)
    {
        db.ChangeTracker.Clear();
        var row = await db.Set<BaseEntity>(removed.GetType()).IgnoreQueryFilters().SingleAsync(x => x.Id == removed.Id);
        row.Active.ShouldBeFalse();
        row.DeletedAt.ShouldNotBeNull();
    }

    private static async Task<BaseEntity> AddRemovedAsync(ApplicationDbContext db, Case @case)
    {
        var entity = @case.Create();
        db.Add(entity);
        await db.SaveChangesAsync(default);
        var remove = entity.GetType().GetMethod("Remove", [entity.GetType()])!;
        remove.Invoke(null, [entity]);
        await db.SaveChangesAsync(default);
        db.ChangeTracker.Clear();
        return entity;
    }

    private static ApplicationDbContext CreateDbContext() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    private static async Task<(int, bool)> Run<TRequest, TResponse>(Endpoint<TRequest, TResponse> endpoint, TRequest request)
        where TRequest : notnull
        where TResponse : WebApi.Common.Models.RestoreResponse
    {
        await endpoint.HandleAsync(request, default);
        return (endpoint.HttpContext.Response.StatusCode, endpoint.Response?.Success ?? false);
    }
}
