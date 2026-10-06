namespace Application.Common.Security;

/// <summary>
/// How a resource is protected. Both transports and both persistence styles (EF Core and Dapper)
/// derive their read filters and write checks from this single table.
/// </summary>
public enum ResourceAccessKind
{
    /// <summary>Shared catalog data: everyone reads, administrators write.</summary>
    Reference,

    /// <summary>User accounts: administrators only.</summary>
    UserAdministration,

    /// <summary>A project: members read and write; any authenticated user may create one and becomes a member.</summary>
    Project,

    /// <summary>Rows with a <c>ProjectId</c>: members of that project read and write.</summary>
    ProjectScoped,

    /// <summary>Rows with an <c>AssignmentId</c>: members of the assignment's project read and write.</summary>
    AssignmentScoped
}

public static class ResourceAccess
{
    private static readonly Dictionary<Type, ResourceAccessKind> Kinds = new()
    {
        [typeof(Organization)] = ResourceAccessKind.Reference,
        [typeof(Workflow)] = ResourceAccessKind.Reference,
        [typeof(AssignmentType)] = ResourceAccessKind.Reference,
        [typeof(Impediment)] = ResourceAccessKind.Reference,
        [typeof(Tenant)] = ResourceAccessKind.Reference,
        [typeof(User)] = ResourceAccessKind.UserAdministration,
        [typeof(Project)] = ResourceAccessKind.Project,
        [typeof(Assignment)] = ResourceAccessKind.ProjectScoped,
        [typeof(UserProject)] = ResourceAccessKind.ProjectScoped,
        [typeof(Appointment)] = ResourceAccessKind.AssignmentScoped,
        [typeof(AssignmentImpediment)] = ResourceAccessKind.AssignmentScoped,
        [typeof(UserAssignment)] = ResourceAccessKind.AssignmentScoped
    };

    public static ResourceAccessKind KindOf(Type entityType) =>
        Kinds.TryGetValue(entityType, out var kind)
            ? kind
            : throw new InvalidOperationException($"No access rule is defined for {entityType.Name}.");

    /// <summary>True when non-administrators may only write rows they own (appointments: their own hours).</summary>
    public static bool IsOwnedByUser(Type entityType) => entityType == typeof(Appointment);

    /// <summary>The access-relevant keys of an entity's current values.</summary>
    public static AccessTarget TargetOf(BaseEntity entity) => entity switch
    {
        Project project => new(typeof(Project), ProjectId: project.Id),
        Assignment assignment => new(typeof(Assignment), ProjectId: assignment.ProjectId),
        UserProject userProject => new(typeof(UserProject), ProjectId: userProject.ProjectId),
        Appointment appointment => new(typeof(Appointment), AssignmentId: appointment.AssignmentId, OwnerUserId: appointment.UserId),
        AssignmentImpediment impediment => new(typeof(AssignmentImpediment), AssignmentId: impediment.AssignmentId),
        UserAssignment userAssignment => new(typeof(UserAssignment), AssignmentId: userAssignment.AssignmentId),
        _ => new(entity.GetType())
    };
}

/// <summary>The keys an access decision depends on.</summary>
public sealed record AccessTarget(Type EntityType, Guid? ProjectId = null, Guid? AssignmentId = null, Guid? OwnerUserId = null);

public enum AccessOperation
{
    /// <summary>Writing a new row with these values.</summary>
    Create,

    /// <summary>Changing or removing an existing row, described by its stored values.</summary>
    Modify,

    /// <summary>The new values of a changed row (for example moving it to another project).</summary>
    Reassign
}

/// <summary>
/// The connection (and transaction) a write is running on, so access checks see the same data and
/// don't take a second pooled connection while a transaction holds the first.
/// </summary>
public sealed record DatabaseSession(System.Data.Common.DbConnection Connection, System.Data.Common.DbTransaction? Transaction);

/// <summary>
/// Enforces <see cref="ResourceAccess"/> for writes. Throws <see cref="AccessDeniedException"/> when the
/// caller may not make the change, and <see cref="Domain.Common.RecordNotFoundException"/> when an
/// existing row isn't visible to them (hidden rows look missing).
/// </summary>
public interface IAccessGuard
{
    Task EnsureCanWriteAsync(AccessTarget target, AccessOperation operation, DatabaseSession? session = null, CancellationToken cancellationToken = default);
}
