namespace IdentityApi.Endpoints.Account.Login;

/// <summary>
/// The sign-in form posted by the WebClient's Astro login page (application/x-www-form-urlencoded).
/// </summary>
public class Request
{
    /// <summary>The login of the user trying to sign in.</summary>
    [DefaultValue("test-user")]
    public string Login { get; set; } = "";

    /// <summary>The password of the user trying to sign in.</summary>
    [DefaultValue("not-too-strong-password")]
    public string Password { get; set; } = "";

    /// <summary>The pending authorization request (the URL the server sent the browser to sign in for).</summary>
    public string AuthRequest { get; set; } = "";
}
