namespace WebApi.Endpoints.Organization.RemoveOrganization;

/// <summary>
/// Request model for removing an organization.
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
/// Response model for the result of removing an organization.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}