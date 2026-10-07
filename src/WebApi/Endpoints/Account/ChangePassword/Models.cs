namespace WebApi.Endpoints.Account.ChangePassword;

/// <summary>
/// Request model for changing the signed-in user's password.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the current password.
    /// </summary>
    [DefaultValue("Current@123")]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    [DefaultValue("Changed@123")]
    public string NewPassword { get; set; } = string.Empty;

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            // Bounded before hashing so oversized inputs can't inflate Argon2 work.
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .MaximumLength(PasswordPolicy.MaximumLength).WithMessage($"Current password must be at most {PasswordPolicy.MaximumLength} characters.");

            RuleFor(x => x.NewPassword)
                .Must(PasswordPolicy.IsSatisfiedBy).WithMessage(PasswordPolicy.Description)
                .NotEqual(x => x.CurrentPassword).WithMessage("The new password must differ from the current password.");
        }
    }
}

/// <summary>
/// Response model for the password change.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the password was changed.
    /// </summary>
    public bool Success { get; set; }
}
