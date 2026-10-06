using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

/// <summary>
/// Resolves the caller from the current request's validated JWT. Without a request there is no
/// caller and nothing is allowed: trusted tools must opt in explicitly with
/// <see cref="StaticCurrentUser.System"/> instead of inheriting access by accident (for example from
/// background work that outlives a request).
/// </summary>
public sealed class HttpContextCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public Guid? UserId => accessor.HttpContext?.User is { Identity.IsAuthenticated: true } user
        && Guid.TryParse(user.FindFirst(CpnucleoClaimTypes.Subject)?.Value, out var id)
            ? id
            : null;

    public bool IsAdmin => accessor.HttpContext?.User is { Identity.IsAuthenticated: true } user
        && user.HasClaim(CpnucleoClaimTypes.Admin, "true");

    public bool IsSystem => false;
}
