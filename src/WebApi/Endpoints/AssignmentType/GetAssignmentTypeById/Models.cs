namespace WebApi.Endpoints.AssignmentType.GetAssignmentTypeById;

/// <summary>
/// Request model for fetching an assignmentType by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentType.
    /// </summary>
    [DefaultValue("6364cf0d-e617-494b-b489-36487a8427f2")]
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
/// Response model for the assignmentType fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the assignmentType details.
    /// </summary>
    public AssignmentTypeDto? AssignmentType { get; set; }
}
