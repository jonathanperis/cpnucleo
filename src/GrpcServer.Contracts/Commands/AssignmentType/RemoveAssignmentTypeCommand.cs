namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for removing an assignmentType.
/// </summary>
public class RemoveAssignmentTypeCommand : ICommand<RemoveAssignmentTypeResult>
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
    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
}
