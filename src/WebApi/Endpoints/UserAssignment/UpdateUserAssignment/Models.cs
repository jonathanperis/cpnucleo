namespace WebApi.Endpoints.UserAssignment.UpdateUserAssignment;

/// <summary>
/// Request model for updating an userAssignment.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the userAssignment.
    /// </summary>
    [DefaultValue("1ac62127-bc1b-484b-85be-e5c9b0ea8d25")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [DefaultValue("35b9c5c1-6abf-4d50-aee8-00abe2f09560")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    [DefaultValue("35f1a233-e070-4205-909d-0eaabf89aec4")]
    public Guid AssignmentId { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.AssignmentId)
                .NotEmpty().WithMessage("AssignmentId is required.");
        }
    }
}

/// <summary>
/// Response model for the updated userAssignment.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
