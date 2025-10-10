namespace GrpcServer.Contracts.Commands.Impediment;

/// <summary>
/// Command model for creating a new impediment.
/// </summary>
public class CreateImpedimentCommand
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
    /// Gets or sets the created impediment.
    /// </summary>
    public ImpedimentDto? Impediment { get; set; }
}
