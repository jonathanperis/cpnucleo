namespace IdentityApi.Endpoints.Refresh;

public class Endpoint(IApplicationDbContext dbContext, IConfiguration configuration) : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Post("/refresh");
        Description(x => x.WithTags("Authentication"));

        Summary(s =>
        {
            s.Summary = "Refresh authenticated user session";
            s.Description = "Issues a new 30-minute JWT for an already authenticated session.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Refreshing JWT token for an active session.");

        if (!Guid.TryParse(User.FindFirst(CpnucleoClaimTypes.Subject)?.Value, out var userId) ||
            !IdentityApi.Security.SessionLifetime.IsRefreshable(User, DateTimeOffset.UtcNow))
        {
            await Send.UnauthorizedAsync(cancellationToken);
            return;
        }

        var account = await dbContext.Users!.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (account is null)
        {
            await Send.UnauthorizedAsync(cancellationToken);
            return;
        }

        var isAdmin = (configuration["CPNUCLEO_ADMIN_LOGINS"] ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Contains(account.Login, StringComparer.OrdinalIgnoreCase);

        var preservedClaims = User.Claims
            .Where(claim => !string.IsNullOrWhiteSpace(claim.Value) &&
                (claim.Type == CpnucleoClaimTypes.Subject ||
                 claim.Type == CpnucleoClaimTypes.UserId ||
                 claim.Type == CpnucleoClaimTypes.SessionStartedAt ||
                 claim.Type == CpnucleoClaimTypes.TenantId ||
                 claim.Type == CpnucleoClaimTypes.TenantSlug ||
                 claim.Type == ClaimTypes.NameIdentifier))
            .Select(claim => (claim.Type, claim.Value))
            .Distinct()
            .ToArray();

        Response.Token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey is required.");
            o.Issuer = configuration["Jwt:Issuer"];
            o.Audience = configuration["Jwt:Audience"];
            var sessionEnd = DateTimeOffset.FromUnixTimeSeconds(long.Parse(User.FindFirst(CpnucleoClaimTypes.SessionStartedAt)!.Value)).AddHours(8).UtcDateTime;
            var accessEnd = DateTime.UtcNow.AddMinutes(30);
            o.ExpireAt = accessEnd < sessionEnd ? accessEnd : sessionEnd;
            foreach (var claim in preservedClaims)
            {
                o.User.Claims.Add(claim);
            }
            if (!string.IsNullOrWhiteSpace(account.Login))
            {
                o.User.Claims.Add((CpnucleoClaimTypes.Login, account.Login));
                o.User.Claims.Add((ClaimTypes.Name, account.Login));
            }
            if (isAdmin) o.User.Claims.Add((CpnucleoClaimTypes.Admin, "true"));
        });

        await Send.OkAsync(Response, cancellationToken);
    }
}
