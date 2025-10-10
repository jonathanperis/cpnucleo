namespace GrpcServer.Contracts.Commands.UserAssignment;

/// <summary>
/// Command model for removing an userAssignment.
/// </summary>
public class RemoveUserAssignmentCommand : ICommand<RemoveUserAssignmentResult>
{
    /// <summary>
    /// Gets or sets the unique identifiers for the userAssignments.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an userAssignment.
/// </summary>
public class RemoveUserAssignmentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
