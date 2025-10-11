namespace GrpcServer.Contracts.Commands.AssignmentImpediment;

/// <summary>
/// Command model for removing an assignmentImpediment.
/// </summary>
public class RemoveAssignmentImpedimentCommand : ICommand<RemoveAssignmentImpedimentResult>
{
    /// <summary>
    /// Gets or sets the unique identifiers for the assignmentImpediments.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an assignmentImpediment.
/// </summary>
public class RemoveAssignmentImpedimentResult
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
