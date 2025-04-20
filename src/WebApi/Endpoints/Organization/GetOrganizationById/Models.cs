namespace WebApi.Endpoints.Organization.GetOrganizationById;

/// <summary>
/// Request model for fetching an organization by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    [DefaultValue("006abcc9-3e72-47e7-a2cd-b4cd755393b2")]
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
/// Response model for the organization fetched by its unique identifier.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets the organization details.
    /// </summary>
    public OrganizationDto? Organization { get; set; }
}