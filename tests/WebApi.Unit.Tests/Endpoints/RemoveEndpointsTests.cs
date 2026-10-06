using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Unit.Tests.Endpoints;

/// <summary>
/// Every EF Core removal endpoint soft-deletes a whole batch in one SaveChanges, or nothing at all.
/// </summary>
public class RemoveEndpointsTests
{
    public sealed record Case(string Name, Func<BaseEntity> Create, Func<IApplicationDbContext, Guid[], Task<(int Status, bool Success)>> Remove)
    {
        public override string ToString() => Name;
    }

    private static readonly Guid AnyId = Guid.NewGuid();

    public static IEnumerable<Case> Cases()
    {
        yield return new("Appointment", () => Appointment.Create("Review", DateTime.UtcNow, 1, AnyId, AnyId),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.Appointment.RemoveAppointment.Endpoint>(db), new WebApi.Endpoints.Appointment.RemoveAppointment.RemoveAppointmentRequest { Ids = [.. ids] }));
        yield return new("Assignment", () => Assignment.Create("Task", "Description", DateTime.UtcNow, DateTime.UtcNow, 1, AnyId, AnyId, AnyId, AnyId),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.Assignment.RemoveAssignment.Endpoint>(db), new WebApi.Endpoints.Assignment.RemoveAssignment.RemoveAssignmentRequest { Ids = [.. ids] }));
        yield return new("AssignmentImpediment", () => AssignmentImpediment.Create("Blocked", AnyId, AnyId),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.AssignmentImpediment.RemoveAssignmentImpediment.Endpoint>(db), new WebApi.Endpoints.AssignmentImpediment.RemoveAssignmentImpediment.RemoveAssignmentImpedimentRequest { Ids = [.. ids] }));
        yield return new("AssignmentType", () => AssignmentType.Create("Exercise"),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.AssignmentType.RemoveAssignmentType.Endpoint>(db), new WebApi.Endpoints.AssignmentType.RemoveAssignmentType.RemoveAssignmentTypeRequest { Ids = [.. ids] }));
        yield return new("Impediment", () => Impediment.Create("Blocked"),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.Impediment.RemoveImpediment.Endpoint>(db), new WebApi.Endpoints.Impediment.RemoveImpediment.RemoveImpedimentRequest { Ids = [.. ids] }));
        yield return new("User", () => User.Create("Learner", $"learner-{Guid.NewGuid():N}", new PasswordHash("hash", "")),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.User.RemoveUser.Endpoint>(db), new WebApi.Endpoints.User.RemoveUser.RemoveUserRequest { Ids = [.. ids] }));
        yield return new("UserAssignment", () => UserAssignment.Create(AnyId, AnyId),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.UserAssignment.RemoveUserAssignment.Endpoint>(db), new WebApi.Endpoints.UserAssignment.RemoveUserAssignment.RemoveUserAssignmentRequest { Ids = [.. ids] }));
        yield return new("UserProject", () => UserProject.Create(AnyId, AnyId),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.UserProject.RemoveUserProject.Endpoint>(db), new WebApi.Endpoints.UserProject.RemoveUserProject.RemoveUserProjectRequest { Ids = [.. ids] }));
        yield return new("Workflow", () => Workflow.Create("Planned", 1),
            (db, ids) => Run(Factory.Create<WebApi.Endpoints.Workflow.RemoveWorkflow.Endpoint>(db), new WebApi.Endpoints.Workflow.RemoveWorkflow.RemoveWorkflowRequest { Ids = [.. ids] }));
    }

    [TestCaseSource(nameof(Cases))]
    public async Task Remove_SoftDeletesEveryRowInTheBatch(Case @case)
    {
        await using var db = CreateDbContext();
        var first = @case.Create();
        var second = @case.Create();
        db.AddRange(first, second);
        await db.SaveChangesAsync(default);

        var (status, success) = await @case.Remove(db, [first.Id, second.Id, first.Id]);

        status.ShouldBe(200);
        success.ShouldBeTrue();
        var rows = await db.Set<BaseEntity>(@case.Create().GetType()).IgnoreQueryFilters().AsNoTracking().ToListAsync();
        rows.ShouldAllBe(row => !row.Active && row.DeletedAt != null, "rows are soft-deleted, never physically removed");
        rows.Count.ShouldBe(2);
    }

    [TestCaseSource(nameof(Cases))]
    public async Task Remove_ChangesNothingWhenAnyIdIsMissing(Case @case)
    {
        await using var db = CreateDbContext();
        var existing = @case.Create();
        db.Add(existing);
        await db.SaveChangesAsync(default);

        var (status, _) = await @case.Remove(db, [existing.Id, Guid.NewGuid()]);

        status.ShouldBe(404);
        db.ChangeTracker.Clear();
        var row = await db.Set<BaseEntity>(existing.GetType()).IgnoreQueryFilters().SingleAsync();
        row.Active.ShouldBeTrue();
        row.DeletedAt.ShouldBeNull();
    }

    private static ApplicationDbContext CreateDbContext() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    private static async Task<(int, bool)> Run<TRequest, TResponse>(Endpoint<TRequest, TResponse> endpoint, TRequest request)
        where TRequest : notnull
        where TResponse : WebApi.Common.Models.RemoveResponse
    {
        endpoint.WithListingServices();
        await endpoint.HandleAsync(request, default);
        return (endpoint.HttpContext.Response.StatusCode, endpoint.Response?.Success ?? false);
    }
}

internal static class DbContextSetExtensions
{
    public static IQueryable<BaseEntity> Set<T>(this ApplicationDbContext db, Type entityType) where T : BaseEntity =>
        (IQueryable<BaseEntity>)typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!
            .MakeGenericMethod(entityType).Invoke(db, null)!;
}
