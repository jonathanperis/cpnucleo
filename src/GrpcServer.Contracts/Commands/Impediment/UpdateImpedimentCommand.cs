namespace GrpcServer.Contracts.Commands.Impediment;

/// <summary>
/// Command model for updating an impediment.
/// </summary>
public class UpdateImpedimentCommand : ICommand<UpdateImpedimentResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the impediment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the impediment.
    /// </summary>
    public required string Name { get; set; }
}

/// <summary>
/// Result model for the updated impediment.
/// </summary>
public class UpdateImpedimentResult
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
