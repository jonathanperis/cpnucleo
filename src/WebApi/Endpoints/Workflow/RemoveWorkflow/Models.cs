namespace WebApi.Endpoints.Workflow.RemoveWorkflow;

/// <summary>
/// Request model for removing a workflow.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of a workflow.
/// </summary>
public class Response : RemoveResponse
{
}
