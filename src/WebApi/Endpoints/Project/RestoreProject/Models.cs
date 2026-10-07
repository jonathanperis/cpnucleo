namespace WebApi.Endpoints.Project.RestoreProject;

/// <summary>
/// Request model for restoring removed projects.
/// </summary>
public class RestoreProjectRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreProjectRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of projects.
/// </summary>
public class Response : RestoreResponse
{
}
