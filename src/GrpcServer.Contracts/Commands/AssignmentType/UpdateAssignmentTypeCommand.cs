namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for updating an assignmentType.
/// </summary>
public class UpdateAssignmentTypeCommand : ICommand<UpdateAssignmentTypeResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentType.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the assignmentType.
    /// </summary>
    public required string Name { get; set; }
}

/// <summary>
/// Result model for the updated assignmentType.
/// </summary>
public class UpdateAssignmentTypeResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
