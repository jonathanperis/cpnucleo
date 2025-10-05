namespace WebApi.Endpoints.Workflow.CreateWorkflow;

/// <summary>
/// Request model for creating a new workflow.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow.
    /// </summary>
    [DefaultValue("873eea4b-55c9-46a6-9512-4d59a77ad28b")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    [DefaultValue("New Workflow")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the order of the workflow.
    /// </summary>
    [DefaultValue(3)]
    public int Order { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Order)
                .NotEmpty().WithMessage("Order is required.");

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0.");
        }
    }
}

/// <summary>
/// Response model for the created workflow.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the created workflow.
    /// </summary>
    public WorkflowDto? Workflow { get; set; }
}
