namespace WebApi.Endpoints.User.RemoveUser;

/// <summary>
/// Request model for removing a user.
/// </summary>
public class RemoveUserRequest : RemoveRequest
{
    public class Validator : RemoveRequestValidator<RemoveUserRequest>
    {
    }
}

/// <summary>
/// Response model for the removal of a user.
/// </summary>
public class Response : RemoveResponse
{
}
