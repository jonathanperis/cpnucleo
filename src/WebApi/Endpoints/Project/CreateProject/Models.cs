namespace WebApi.Endpoints.Project.CreateProject;

/// <summary>
/// Request model for creating a new project.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    [DefaultValue("8de21ef6-19a3-41ee-b4cd-ea3fae2e91c9")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    [DefaultValue("New Project")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    [DefaultValue("006abcc9-3e72-47e7-a2cd-b4cd755393b2")]
    public Guid OrganizationId { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("OrganizationId is required.");                
        }
    }    
}

/// <summary>
/// Response model for the created project.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets the created project.
    /// </summary>
    public ProjectDto? Project { get; set; }
}