namespace WebApi.Endpoints.AssignmentType.UpdateAssignmentType;

/// <summary>
/// Request model for updating an assignmentType.
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
    [DefaultValue("Updated AssignmentType")]
    public required string Name { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}

/// <summary>
/// Response model for the updated assignmentType.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
