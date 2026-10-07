namespace WebApi.Endpoints.Account.GetMe;

/// <summary>
/// The signed-in user's profile. Credentials are never returned.
/// </summary>
public class Response
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Login { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
