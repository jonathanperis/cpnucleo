namespace WebApi.Endpoints.User.ListUsers;

/// <summary>
/// Request model for listing users.
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
            RuleFor(x => x.Pagination).ValidPagination(typeof(Domain.Entities.User));
        }
    }    
}

/// <summary>
/// Response model for the list of users.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of users.
    /// </summary>
    public PaginatedResult<UserDto?> Result { get; set; } = null!;
}
