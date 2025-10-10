namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for creating a new assignmentType.
/// </summary>
public class CreateAssignmentTypeCommand
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
/// Result model for the created assignmentType.
/// </summary>
public class CreateAssignmentTypeResult
{
    /// <summary>
    /// Gets or sets the created assignmentType.
    /// </summary>
    public AssignmentTypeDto? AssignmentType { get; set; }
}
