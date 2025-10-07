namespace WebApi.Endpoints.AssignmentType.CreateAssignmentType;

/// <summary>
/// Request model for creating a new assignmentType.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentType.
    /// </summary>
    [DefaultValue("6364cf0d-e617-494b-b489-36487a8427f2")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the assignmentType.
    /// </summary>
    [DefaultValue("New AssignmentType")]
    public required string Name { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}

/// <summary>
/// Response model for the created assignmentType.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the created assignmentType.
    /// </summary>
    public AssignmentTypeDto? AssignmentType { get; set; }
}
