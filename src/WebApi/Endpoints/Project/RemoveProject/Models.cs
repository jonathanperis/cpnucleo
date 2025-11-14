namespace WebApi.Endpoints.Project.RemoveProject;

/// <summary>
/// Request model for removing a project.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of a project.
/// </summary>
public class Response : RemoveResponse
{
}
