namespace IdentityApi.Endpoints.Refresh;

public class Endpoint(IApplicationDbContext dbContext, TokenIssuer tokenIssuer) : EndpointWithoutRequest<Response>
{
    private static readonly string[] PreservedClaimTypes =
    [
        CpnucleoClaimTypes.Subject,
        CpnucleoClaimTypes.UserId,
        CpnucleoClaimTypes.SessionStartedAt,
        CpnucleoClaimTypes.TenantId,
        CpnucleoClaimTypes.TenantSlug,
        ClaimTypes.NameIdentifier
    ];

    public override void Configure()
    {
        Post("/refresh");
        Description(x => x.WithTags("Authentication"));

        Summary(s =>
        {
            s.Summary = "Refresh authenticated user session";
            s.Description = "Issues a new 30-minute JWT for an already authenticated session, never beyond its eight-hour boundary. Fails when the account is inactive or its credentials changed since the session started.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var now = tokenIssuer.Now;
        if (!Guid.TryParse(User.FindFirst(CpnucleoClaimTypes.Subject)?.Value, out var userId) ||
            !SessionLifetime.IsRefreshable(User, now))
        {
            await Send.UnauthorizedEnvelopeAsync("The session has ended. Sign in again.", cancellationToken);
            return;
        }

        // The global query filter excludes inactive accounts.
        var account = await dbContext.Users!.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (account is null ||
            !string.Equals(User.FindFirst(CpnucleoClaimTypes.SecurityStamp)?.Value, SecurityStamp.Compute(account), StringComparison.Ordinal))
        {
            Logger.LogInformation("Refresh rejected for user {UserId}: inactive account or changed credentials.", userId);
            await Send.UnauthorizedEnvelopeAsync("The session is no longer valid. Sign in again.", cancellationToken);
            return;
        }

        var preservedClaims = User.Claims
            .Where(claim => !string.IsNullOrWhiteSpace(claim.Value) && PreservedClaimTypes.Contains(claim.Type))
            .Select(claim => (claim.Type, claim.Value))
            .ToArray();

        var accessEnd = now + TokenIssuer.AccessTokenLifetime;
        var sessionEnd = SessionLifetime.EndsAt(User);
        Response.Token = tokenIssuer.Issue(account, preservedClaims, accessEnd < sessionEnd ? accessEnd : sessionEnd);

        await Send.OkAsync(Response, cancellationToken);
    }
}
