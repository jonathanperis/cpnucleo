namespace GrpcServer.Contracts.Commands.UserAssignment;

/// <summary>
/// Command model for updating an userAssignment.
/// </summary>
public class UpdateUserAssignmentCommand : ICommand<UpdateUserAssignmentResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the userAssignment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    public Guid AssignmentId { get; set; }
}

/// <summary>
/// Result model for the updated userAssignment.
/// </summary>
public class UpdateUserAssignmentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
