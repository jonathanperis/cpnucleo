namespace IdentityApi.Endpoints.Login;

/// <summary>
/// Represents a user login request.
/// </summary>
public class Request
{
    /// <summary>
    /// The username of the user trying to log in.
    /// </summary>
    [DefaultValue("test-user")]
    public required string Login { get; set; }
    
    /// <summary>
    /// The password of the user trying to log in.
    /// </summary>
    [DefaultValue("not-too-strong-password")]
    public required string Password { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required.");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}

/// <summary>
/// Represents a user login response.
/// </summary>
public class Response
{
    /// <summary>
    /// 
    /// </summary>
    public string? Token { get; set; }
}