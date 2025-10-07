namespace WebApi.Endpoints.Workflow.ListWorkflows;

/// <summary>
/// Request model for listing workflows.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
    
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination)
                .NotNull().WithMessage("Pagination is required.");;
        }
    }    
}

/// <summary>
/// Response model for the list of workflows.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of workflows.
    /// </summary>
    public required PaginatedResult<WorkflowDto?> Result { get; set; }
}
