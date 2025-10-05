namespace WebApi.Endpoints.User.ListUsers;

/// <summary>
/// Request model for listing users.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
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
