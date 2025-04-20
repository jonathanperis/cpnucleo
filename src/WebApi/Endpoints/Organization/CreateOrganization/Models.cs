namespace WebApi.Endpoints.Organization.CreateOrganization;

/// <summary>
/// Request model for creating a new organization.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    [DefaultValue("006abcc9-3e72-47e7-a2cd-b4cd755393b2")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    [DefaultValue("New Organization")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the organization.
    /// </summary>
    [DefaultValue("Organization description goes here")]
    public string? Description { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");
        }
    }    
}

/// <summary>
/// Response model for the created organization.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets the created organization.
    /// </summary>
    public OrganizationDto? Organization { get; set; }
}