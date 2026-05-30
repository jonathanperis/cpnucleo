namespace IdentityApi.Endpoints.Refresh;

public class Endpoint : EndpointWithoutRequest<Response>
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

        var preservedClaims = User.Claims
            .Where(claim => !string.IsNullOrWhiteSpace(claim.Value) &&
                (claim.Type == CpnucleoClaimTypes.Subject ||
                 claim.Type == CpnucleoClaimTypes.UserId ||
                 claim.Type == CpnucleoClaimTypes.Login ||
                 claim.Type == CpnucleoClaimTypes.TenantId ||
                 claim.Type == CpnucleoClaimTypes.TenantSlug ||
                 claim.Type == CpnucleoClaimTypes.Admin ||
                 claim.Type == ClaimTypes.NameIdentifier ||
                 claim.Type == ClaimTypes.Name))
            .Select(claim => (claim.Type, claim.Value))
            .Distinct()
            .ToArray();

        Response.Token = JwtBearer.CreateToken(o =>
        {
            o.ExpireAt = DateTime.UtcNow.AddMinutes(30);
            foreach (var claim in preservedClaims)
            {
                o.User.Claims.Add(claim);
            }
        });

        await Send.OkAsync(Response, cancellationToken);
    }
}
