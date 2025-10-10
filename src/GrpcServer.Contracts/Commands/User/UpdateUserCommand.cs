namespace GrpcServer.Contracts.Commands.User;

/// <summary>
/// Command model for updating a user.
/// </summary>
public class UpdateUserCommand : ICommand<UpdateUserResult>
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
    /// Gets or sets the password of the user.
    /// </summary>
    public required string Password { get; set; }
}

/// <summary>
/// Result model for the updated user.
/// </summary>
public class UpdateUserResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
