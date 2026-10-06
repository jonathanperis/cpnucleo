namespace WebApi.Endpoints.Workflow.RemoveWorkflow;

/// <summary>
/// Request model for removing a workflow.
/// </summary>
public class RemoveWorkflowRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveWorkflowRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of a workflow.
/// </summary>
public class Response : RemoveResponse
{
}
