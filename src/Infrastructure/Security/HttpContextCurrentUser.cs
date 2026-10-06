using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

/// <summary>
/// Resolves the caller from the current request's validated JWT. Code running without any request
/// (CLI tools, seeding, migrations) is trusted system work.
/// </summary>
public sealed class HttpContextCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public Guid? UserId => accessor.HttpContext?.User is { Identity.IsAuthenticated: true } user
        && Guid.TryParse(user.FindFirst(CpnucleoClaimTypes.Subject)?.Value, out var id)
            ? id
            : null;

    public bool IsAdmin => accessor.HttpContext?.User is { Identity.IsAuthenticated: true } user
        && user.HasClaim(CpnucleoClaimTypes.Admin, "true");

    public bool IsSystem => accessor.HttpContext is null;
}
