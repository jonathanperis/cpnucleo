namespace WebApi.Endpoints.UserProject.RemoveUserProject;

/// <summary>
/// Request model for removing a user project.
/// </summary>
public class RemoveUserProjectRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveUserProjectRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of a user project.
/// </summary>
public class Response : RemoveResponse
{
}
