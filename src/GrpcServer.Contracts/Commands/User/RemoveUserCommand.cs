namespace GrpcServer.Contracts.Commands.User;

/// <summary>
/// Command model for removing a user.
/// </summary>
public class RemoveUserCommand
{
    /// <summary>
    /// Gets or sets the unique identifiers for the users.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of a user.
/// </summary>
public class RemoveUserResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
