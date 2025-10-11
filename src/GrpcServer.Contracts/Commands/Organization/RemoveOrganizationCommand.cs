namespace GrpcServer.Contracts.Commands.Organization;

/// <summary>
/// Command model for removing an organization.
/// </summary>
public class RemoveOrganizationCommand : ICommand<RemoveOrganizationResult>
{
    /// <summary>
    /// Gets or sets the unique identifiers for the organizations.
    /// </summary>
    public required List<Guid> Ids { get; set; }
}

/// <summary>
/// Result model for the removal of an organization.
/// </summary>
public class RemoveOrganizationResult
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