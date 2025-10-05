namespace WebApi.Endpoints.User.GetUserById;

/// <summary>
/// Request model for fetching an user by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [DefaultValue("35b9c5c1-6abf-4d50-aee8-00abe2f09560")]
    public Guid Id { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}

/// <summary>
/// Response model for the user fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the user details.
    /// </summary>
    public UserDto? User { get; set; }
}
