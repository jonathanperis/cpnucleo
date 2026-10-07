namespace WebApi.Endpoints.UserProject.RestoreUserProject;

/// <summary>
/// Request model for restoring removed user projects.
/// </summary>
public class RestoreUserProjectRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreUserProjectRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of user projects.
/// </summary>
public class Response : RestoreResponse
{
}
