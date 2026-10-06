namespace Infrastructure.Http;

public enum DatabaseErrorKind
{
    Duplicate,
    InactiveReference,
    ActiveDependents,
    InvalidValue,
    ConcurrentChange
}

public sealed record DatabaseError(DatabaseErrorKind Kind, string Message, string? Field);

/// <summary>
/// Maps PostgreSQL integrity errors (including the relationship triggers) to client-safe messages,
/// shared by the REST middleware and the gRPC interceptor.
/// </summary>
public static class DatabaseErrors
{
    public static DatabaseError? Classify(Exception exception)
    {
        var postgres = exception as PostgresException ?? exception.InnerException as PostgresException;
        return postgres?.SqlState switch
        {
            PostgresErrorCodes.UniqueViolation => new(DatabaseErrorKind.Duplicate,
                postgres.ConstraintName == "Users_ActiveLogin" ? "This login is already in use." : "A record with this identity already exists.",
                postgres.ConstraintName == "Users_ActiveLogin" ? "Login" : null),
            PostgresErrorCodes.ForeignKeyViolation => new(DatabaseErrorKind.InactiveReference,
                "A related record is missing or has been removed.", postgres.ColumnName),
            PostgresErrorCodes.RestrictViolation => new(DatabaseErrorKind.ActiveDependents,
                $"This record still has active {Describe(postgres.TableName)}. Remove them first.", null),
            PostgresErrorCodes.CheckViolation or PostgresErrorCodes.NotNullViolation => new(DatabaseErrorKind.InvalidValue,
                "A value is missing or invalid.", postgres.ColumnName),
            PostgresErrorCodes.DeadlockDetected or PostgresErrorCodes.SerializationFailure => new(DatabaseErrorKind.ConcurrentChange,
                "The records changed concurrently. Retry the request.", null),
            _ => null
        };
    }

    private static string Describe(string? table) => table switch
    {
        null or "" => "related records",
        "AssignmentImpediments" => "assignment impediments",
        "AssignmentTypes" => "assignment types",
        "UserAssignments" => "user assignments",
        "UserProjects" => "project memberships",
        _ => table.ToLowerInvariant()
    };
}
