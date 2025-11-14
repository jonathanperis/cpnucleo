namespace WebApi.Endpoints.UserProject.RemoveUserProject;

/// <summary>
/// Request model for removing a user project.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of a user project.
/// </summary>
public class Response : RemoveResponse
{
}
