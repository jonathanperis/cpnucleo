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
                .NotEmpty().WithMessage("Login is required.")
                .MaximumLength(Domain.Common.Guard.LoginMaxLength).WithMessage($"Login must be at most {Domain.Common.Guard.LoginMaxLength} characters.");

            // Bounded before hashing so oversized inputs can't inflate Argon2 work.
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(PasswordPolicy.MaximumLength).WithMessage($"Password must be at most {PasswordPolicy.MaximumLength} characters.");
        }
    }
}

/// <summary>
/// Represents a user login response.
/// </summary>
public class Response
{
    /// <summary>
    /// The signed access token (30 minutes) for the authenticated session.
    /// </summary>
    public string? Token { get; set; }
}
