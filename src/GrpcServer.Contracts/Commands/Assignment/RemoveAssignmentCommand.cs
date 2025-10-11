namespace GrpcServer.Contracts.Commands.Assignment;

/// <summary>
/// Command model for removing an assignment.
/// </summary>
public class RemoveAssignmentCommand : ICommand<RemoveAssignmentResult>
{
    /// <summary>
    /// Gets or sets the unique identifiers for the assignments.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an assignment.
/// </summary>
public class RemoveAssignmentResult
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
