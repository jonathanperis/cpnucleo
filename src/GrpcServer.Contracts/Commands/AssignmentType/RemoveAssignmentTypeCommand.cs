namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for removing an assignmentType.
/// </summary>
public class RemoveAssignmentTypeCommand
{
    /// <summary>
    /// Gets or sets the unique identifiers for the assignmentTypes.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an assignmentType.
/// </summary>
public class RemoveAssignmentTypeResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
