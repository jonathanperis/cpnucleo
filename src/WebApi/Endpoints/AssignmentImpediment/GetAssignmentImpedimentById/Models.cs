namespace WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById;

/// <summary>
/// Request model for fetching an assignmentImpediment by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentImpediment.
    /// </summary>
    [DefaultValue("7d1ce93c-54be-4826-a947-a972c10db2ad")]
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
/// Response model for the assignmentImpediment fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the assignmentImpediment details.
    /// </summary>
    public AssignmentImpedimentDto? AssignmentImpediment { get; set; }
}
