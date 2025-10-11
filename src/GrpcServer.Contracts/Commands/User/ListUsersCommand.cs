namespace GrpcServer.Contracts.Commands.User;

/// <summary>
/// Command model for listing users.
/// </summary>
public class ListUsersCommand : ICommand<ListUsersResult>
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
    /// Gets or sets a value indicating whether the list was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the paginated result of users.
    /// </summary>
    public required PaginatedResult<UserDto?> Result { get; set; }
}
