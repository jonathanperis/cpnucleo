namespace WebApi.Endpoints.Assignment.UpdateAssignment;

/// <summary>
/// Request model for updating an assignment.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    [DefaultValue("35f1a233-e070-4205-909d-0eaabf89aec4")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the assignment.
    /// </summary>
    [DefaultValue("Updated Assignment")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the assignment.
    /// </summary>
    [DefaultValue("Updated Assignment Description goes here")]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the start date of the assignment.
    /// </summary>
    [DefaultValue("2069-04-20")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the assignment.
    /// </summary>
    [DefaultValue("2069-04-20")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the amount of hours for the assignment.
    /// </summary>
    [DefaultValue(6)]
    public int AmountHours { get; set; }

    /// <summary>
    /// Gets or sets the project identifier for the assignment.
    /// </summary>
    [DefaultValue("8de21ef6-19a3-41ee-b4cd-ea3fae2e91c9")]
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the workflow identifier for the assignment.
    /// </summary>
    [DefaultValue("873eea4b-55c9-46a6-9512-4d59a77ad28b")]
    public Guid WorkflowId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier for the assignment.
    /// </summary>
    [DefaultValue("35b9c5c1-6abf-4d50-aee8-00abe2f09560")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the assignment type identifier for the assignment.
    /// </summary>
    [DefaultValue("6364cf0d-e617-494b-b489-36487a8427f2")]
    public Guid AssignmentTypeId { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.");

            RuleFor(x => x.AmountHours)
                .NotEmpty().WithMessage("AmountHours is required.");

            RuleFor(x => x.AmountHours)
               .GreaterThan(0).WithMessage("AmountHours must be greater than 0.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");

            RuleFor(x => x.WorkflowId)
                .NotEmpty().WithMessage("WorkflowId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.AssignmentTypeId)
                .NotEmpty().WithMessage("AssignmentTypeId is required.");
        }
    }
}

/// <summary>
/// Response model for the updated assignment.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
