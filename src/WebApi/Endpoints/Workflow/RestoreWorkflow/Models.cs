namespace WebApi.Endpoints.Workflow.RestoreWorkflow;

/// <summary>
/// Request model for restoring removed workflows.
/// </summary>
public class RestoreWorkflowRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreWorkflowRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of workflows.
/// </summary>
public class Response : RestoreResponse
{
}
