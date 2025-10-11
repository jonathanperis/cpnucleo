namespace GrpcServer.Contracts.Commands.Impediment;

/// <summary>
/// Command model for fetching an impediment by its unique identifier.
/// </summary>
public class GetImpedimentByIdCommand : ICommand<GetImpedimentByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the impediment.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the impediment fetched by its unique identifier.
/// </summary>
public class GetImpedimentByIdResult
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
    /// Gets or sets the impediment details.
    /// </summary>
    public ImpedimentDto? Impediment { get; set; }
}
