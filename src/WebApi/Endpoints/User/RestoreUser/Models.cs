namespace WebApi.Endpoints.User.RestoreUser;

/// <summary>
/// Request model for restoring removed users.
/// </summary>
public class RestoreUserRequest : RestoreRequest
{
    public class Validator : RestoreRequestValidator<RestoreUserRequest>
    {
    }
}

/// <summary>
/// Response model for the restore of users.
/// </summary>
public class Response : RestoreResponse
{
}
