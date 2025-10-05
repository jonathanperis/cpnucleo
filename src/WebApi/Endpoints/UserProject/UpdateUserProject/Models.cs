namespace WebApi.Endpoints.UserProject.UpdateUserProject;

/// <summary>
/// Request model for updating an userProject.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the userProject.
    /// </summary>
    [DefaultValue("ad45c2fd-0320-4cfc-b516-64da8f3cda11")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [DefaultValue("35b9c5c1-6abf-4d50-aee8-00abe2f09560")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    [DefaultValue("8de21ef6-19a3-41ee-b4cd-ea3fae2e91c9")]
    public Guid ProjectId { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}

/// <summary>
/// Response model for the updated userProject.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
