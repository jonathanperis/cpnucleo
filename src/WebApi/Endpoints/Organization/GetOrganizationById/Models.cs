namespace WebApi.Endpoints.Organization.GetOrganizationById;

/// <summary>
/// Request model for getting an organization.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    [DefaultValue("Guid.NewGuid()")]
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
/// Response model containing the requested organization's data.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets the created organization's data transfer object.
    /// </summary>
    public OrganizationDto? Organization { get; set; }
}