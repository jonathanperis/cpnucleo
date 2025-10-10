namespace GrpcServer.Contracts.Commands.Impediment;

/// <summary>
/// Command model for removing an impediment.
/// </summary>
public class RemoveImpedimentCommand
{
    /// <summary>
    /// Gets or sets the unique identifiers for the impediments.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an impediment.
/// </summary>
public class RemoveImpedimentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}
