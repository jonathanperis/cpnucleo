namespace WebApi.Endpoints.UserProject.GetUserProjectById;

/// <summary>
/// Request model for fetching an userProject by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the userProject.
    /// </summary>
    [DefaultValue("ad45c2fd-0320-4cfc-b516-64da8f3cda11")]
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
/// Response model for the userProject fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the userProject details.
    /// </summary>
    public UserProjectDto? UserProject { get; set; }
}
