namespace GrpcServer.Contracts.Commands.User;

/// <summary>
/// Command model for listing users.
/// </summary>
public class ListUsersCommand
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
}

/// <summary>
/// Result model for the list of users.
/// </summary>
public class ListUsersResult
{
    /// <summary>
    /// Gets or sets the paginated result of users.
    /// </summary>
    public required PaginatedResult<UserDto?> Result { get; set; }
}
