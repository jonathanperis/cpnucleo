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
/// Response model for the list of users.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of users.
    /// </summary>
    public required PaginatedResult<UserDto?> Result { get; set; }
}
