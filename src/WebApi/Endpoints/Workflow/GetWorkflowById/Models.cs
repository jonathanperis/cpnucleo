namespace WebApi.Endpoints.Workflow.GetWorkflowById;

/// <summary>
/// Request model for fetching an workflow by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow.
    /// </summary>
    [DefaultValue("873eea4b-55c9-46a6-9512-4d59a77ad28b")]
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
/// Response model for the workflow fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the workflow details.
    /// </summary>
    public WorkflowDto? Workflow { get; set; }
}
