namespace WebApi.Endpoints.Account.UpdateMe;

/// <summary>
/// Request model for changing the signed-in user's display name.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the new display name.
    /// </summary>
    [DefaultValue("Updated Name")]
    public string? Name { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}

/// <summary>
/// Response model for the display name change.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
