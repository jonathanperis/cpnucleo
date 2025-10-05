namespace WebApi.Endpoints.Assignment.GetAssignmentById;

/// <summary>
/// Request model for fetching an assignment by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    [DefaultValue("35f1a233-e070-4205-909d-0eaabf89aec4")]
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
/// Response model for the assignment fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the assignment details.
    /// </summary>
    public AssignmentDto? Assignment { get; set; }
}
