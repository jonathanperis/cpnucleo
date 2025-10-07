namespace WebApi.Endpoints.Project.ListProjects;

/// <summary>
/// Request model for listing projects.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
    
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination)
                .NotNull().WithMessage("Pagination is required.");;
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
    public required PaginatedResult<ProjectDto?> Result { get; set; }
}
