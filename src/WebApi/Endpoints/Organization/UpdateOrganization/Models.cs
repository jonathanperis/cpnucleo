namespace WebApi.Endpoints.Organization.UpdateOrganization;

/// <summary>
/// Request model for updating an organization.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    [DefaultValue("Guid.NewGuid()")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    [DefaultValue("Updated Organization")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the organization.
    /// </summary>
    [DefaultValue("Updated organization description goes here")]
    public string? Description { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
                        
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");
        }
    }    
}

/// <summary>
/// Response model for the result of updating an organization.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}