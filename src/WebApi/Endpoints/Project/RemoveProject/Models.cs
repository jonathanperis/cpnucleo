namespace WebApi.Endpoints.Project.RemoveProject;

/// <summary>
/// Request model for removing a project.
/// </summary>
public class RemoveProjectRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveProjectRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of a project.
/// </summary>
public class Response : RemoveResponse
{
}
