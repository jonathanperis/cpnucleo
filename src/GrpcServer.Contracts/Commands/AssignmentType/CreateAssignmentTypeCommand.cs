namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for creating a new assignmentType.
/// </summary>
public class CreateAssignmentTypeCommand : ICommand<CreateAssignmentTypeResult>
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
    /// Gets or sets a value indicating whether the creation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the created assignmentType.
    /// </summary>
    public AssignmentTypeDto? AssignmentType { get; set; }
}
