namespace GrpcServer.Contracts.Commands.User;

/// <summary>
/// Command model for creating a new user.
/// </summary>
public class CreateUserCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the login of the user.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public required string Password { get; set; }
}

/// <summary>
/// Result model for the created user.
/// </summary>
public class CreateUserResult
{
    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    public UserDto? User { get; set; }
}
