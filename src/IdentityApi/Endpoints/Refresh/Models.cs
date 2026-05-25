namespace IdentityApi.Endpoints.Refresh;

/// <summary>
/// Represents a refreshed user session response.
/// </summary>
public class Response
{
    /// <summary>
    /// A renewed 30-minute JWT for the authenticated user session.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
