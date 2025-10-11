namespace GrpcServer.Contracts.Commands.Organization;

/// <summary>
/// Command model for listing organizations.
/// </summary>
public class ListOrganizationsCommand : ICommand<ListOrganizationsResult>
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of organizations.
/// </summary>
public class ListOrganizationsResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the list was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the paginated result of organizations.
    /// </summary>
    public PaginatedResult<OrganizationDto?> Result { get; set; }
}
