namespace GrpcServer.Contracts.Commands.Assignment;

/// <summary>
/// Command model for updating an assignment.
/// </summary>
public class UpdateAssignmentCommand : ICommand<UpdateAssignmentResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the assignment.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the assignment.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the start date of the assignment.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the assignment.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the amount of hours for the assignment.
    /// </summary>
    public int AmountHours { get; set; }

    /// <summary>
    /// Gets or sets the project identifier for the assignment.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the workflow identifier for the assignment.
    /// </summary>
    public Guid WorkflowId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier for the assignment.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the assignment type identifier for the assignment.
    /// </summary>
    public Guid AssignmentTypeId { get; set; }
}

/// <summary>
/// Result model for the updated assignment.
/// </summary>
public class UpdateAssignmentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
}
