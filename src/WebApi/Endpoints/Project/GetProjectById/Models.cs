namespace WebApi.Endpoints.Project.GetProjectById;

/// <summary>
/// Request model for fetching an project by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    [DefaultValue("8de21ef6-19a3-41ee-b4cd-ea3fae2e91c9")]
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
/// Response model for the project fetched by its unique identifier.
/// </summary>
public class Response
{   
    /// <summary>
    /// Gets or sets the project details.
    /// </summary>
    public ProjectDto? Project { get; set; }
}