namespace WebApi.Endpoints.User.CreateUser;

/// <summary>
/// Request model for creating a new user.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [DefaultValue("35b9c5c1-6abf-4d50-aee8-00abe2f09560")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    [DefaultValue("New User")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the login of the user.
    /// </summary>
    [DefaultValue("verysecretusername")]
    public required string Login { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [DefaultValue("veryverysecretpassword")]
    public required string Password { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }
    }
}

/// <summary>
/// Response model for the created user.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    public UserDto? User { get; set; }
}
