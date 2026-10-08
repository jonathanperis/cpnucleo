namespace WebApi.Endpoints.Project.ListProjects;

/// <summary>
/// Request model for listing projects.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    [FromQuery]
    public PaginationParams Pagination { get; set; } = new();
        
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).ValidPagination(typeof(Domain.Entities.Project));
        }
    }    
}

/// <summary>
/// Response model for the list of projects.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of projects.
    /// </summary>
    public PaginatedResult<ProjectDto?> Result { get; set; } = null!;
}
