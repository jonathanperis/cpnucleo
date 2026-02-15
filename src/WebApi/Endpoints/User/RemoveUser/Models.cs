namespace WebApi.Endpoints.User.RemoveUser;

/// <summary>
/// Request model for removing a user.
/// </summary>
public class Request : RemoveRequest
{
    public new class Validator : RemoveRequest.Validator
    {
    }
}

/// <summary>
/// Response model for the removal of a user.
/// </summary>
public class Response : RemoveResponse
{
}
