namespace WebApi.Endpoints.Organization.RemoveOrganization;

/// <summary>
/// Request model for removing an organization.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifiers for the organizations.
    /// </summary>
    public List<Guid>? Ids { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Ids)
                .NotEmpty().WithMessage("Ids are required.");
            RuleForEach(x => x.Ids)
                .NotEmpty().WithMessage("Each Id is required.");
        }
    }    
}

/// <summary>
/// Response model for the removal of an organization.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets a value indicating whether the removal was successful.
    /// </summary>
    public bool Success { get; set; }
}