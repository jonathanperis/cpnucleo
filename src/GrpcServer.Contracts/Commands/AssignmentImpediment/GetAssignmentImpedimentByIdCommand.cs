namespace GrpcServer.Contracts.Commands.AssignmentImpediment;

/// <summary>
/// Command model for fetching an assignmentImpediment by its unique identifier.
/// </summary>
public class GetAssignmentImpedimentByIdCommand : ICommand<GetAssignmentImpedimentByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentImpediment.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the assignmentImpediment fetched by its unique identifier.
/// </summary>
public class GetAssignmentImpedimentByIdResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the fetch was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the assignmentImpediment details.
    /// </summary>
    public AssignmentImpedimentDto? AssignmentImpediment { get; set; }
}
