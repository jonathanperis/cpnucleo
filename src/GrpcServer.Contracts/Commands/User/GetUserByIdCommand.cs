namespace GrpcServer.Contracts.Commands.User;

/// <summary>
/// Command model for fetching a user by its unique identifier.
/// </summary>
public class GetUserByIdCommand : ICommand<GetUserByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the user fetched by its unique identifier.
/// </summary>
public class GetUserByIdResult
{
    /// <summary>
    /// Gets or sets the user details.
    /// </summary>
    public UserDto? User { get; set; }
}
