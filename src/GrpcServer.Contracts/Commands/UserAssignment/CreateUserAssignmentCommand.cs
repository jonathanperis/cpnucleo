namespace GrpcServer.Contracts.Commands.UserAssignment;

/// <summary>
/// Command model for creating a new userAssignment.
/// </summary>
public class CreateUserAssignmentCommand : ICommand<CreateUserAssignmentResult>
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
/// Result model for the created userAssignment.
/// </summary>
public class CreateUserAssignmentResult
{
    /// <summary>
    /// Gets or sets the created userAssignment.
    /// </summary>
    public UserAssignmentDto? UserAssignment { get; set; }
}
