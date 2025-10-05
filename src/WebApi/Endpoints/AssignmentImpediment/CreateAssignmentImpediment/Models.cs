namespace WebApi.Endpoints.AssignmentImpediment.CreateAssignmentImpediment;

/// <summary>
/// Request model for creating a new assignmentImpediment.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentImpediment.
    /// </summary>
    [DefaultValue("7d1ce93c-54be-4826-a947-a972c10db2ad")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the assignmentImpediment.
    /// </summary>
    [DefaultValue("AssignmentImpediment description goes here")]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    [DefaultValue("edab4f0b-a8eb-4d01-ae7a-a11fc3f56d3a")]
    public Guid AssignmentId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the impediment.
    /// </summary>
    [DefaultValue("6da4950a-fac3-4578-8ee0-b05aa15eae35")]
    public Guid ImpedimentId { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.AssignmentId)
                .NotEmpty().WithMessage("AssignmentId is required.");

            RuleFor(x => x.ImpedimentId)
                .NotEmpty().WithMessage("ImpedimentId is required.");
        }
    }
}

/// <summary>
/// Response model for the created assignmentImpediment.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the created assignmentImpediment.
    /// </summary>
    public AssignmentImpedimentDto? AssignmentImpediment { get; set; }
}
