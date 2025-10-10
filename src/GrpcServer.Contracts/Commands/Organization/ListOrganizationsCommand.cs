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
    /// Gets or sets the paginated result of organizations.
    /// </summary>
    public PaginatedResult<OrganizationDto?> Result { get; set; }
}
