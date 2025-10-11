namespace GrpcServer.Contracts.Commands.Impediment;

/// <summary>
/// Command model for creating a new impediment.
/// </summary>
public class CreateImpedimentCommand : ICommand<CreateImpedimentResult>
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
/// Result model for the created impediment.
/// </summary>
public class CreateImpedimentResult
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
    /// Gets or sets the created impediment.
    /// </summary>
    public ImpedimentDto? Impediment { get; set; }
}
