namespace IdentityApi.Endpoints.Login;

public class Endpoint(
    IApplicationDbContext dbContext,
    TimingSafePasswordCheck passwordCheck,
    LoginThrottle throttle,
    TokenIssuer tokenIssuer) : Endpoint<Request, Response>
{
    public const string ConcurrencyPolicy = "login-concurrency";

    public override void Configure()
    {
        Post("/login");
        Description(x => x.WithTags("Authentication"));
        AllowAnonymous();
        // Each attempt costs an Argon2id hash (64 MiB); cap how many run at once.
        Options(x => x.RequireRateLimiting(ConcurrencyPolicy));

        Summary(s =>
        {
            s.Summary = "Authenticate user and generate JWT token";
            s.Description =
                "Authenticates the user based on provided credentials and generates a JWT token upon successful authentication.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        // Login names are personal data (and sometimes mistyped passwords): they are never logged.
        var normalizedLogin = req.Login.Trim().ToLowerInvariant();
        if (throttle.RetryAfter(normalizedLogin) is { } retryAfter)
        {
            Logger.LogWarning("Login rejected: too many failed attempts for this login.");
            var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
            HttpContext.Response.Headers.RetryAfter = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
            await ApiErrors.WriteAsync(HttpContext, StatusCodes.Status429TooManyRequests,
                $"Too many failed sign-in attempts. Try again in {seconds} seconds.", cancellationToken: cancellationToken);
            return;
        }

        var matches = await dbContext.Users!
            .Where(u => u.Login != null && u.Login.Trim().ToLower() == normalizedLogin)
            .Take(2).ToListAsync(cancellationToken);
        // Ambiguous legacy logins never authenticate an arbitrary account.
        var item = matches.Count == 1 ? matches[0] : null;

        if (!passwordCheck.Verify(req.Password, item?.Password) || item is null)
        {
            throttle.RecordFailure(normalizedLogin);
            Logger.LogWarning("Login failed: unknown, ambiguous or wrong credentials.");
            await Send.NotFoundEnvelopeAsync("Invalid login or password.", cancellationToken);
            return;
        }

        throttle.RecordSuccess(normalizedLogin);

        // Deterministic tenant hint: the user's earliest active membership.
        var tenantId = await (from userProject in dbContext.UserProjects!
                join project in dbContext.Projects! on userProject.ProjectId equals project.Id
                where userProject.UserId == item.Id
                orderby userProject.CreatedAt, userProject.Id
                select project.OrganizationId)
            .FirstOrDefaultAsync(cancellationToken);

        var now = tokenIssuer.Now;
        var sessionClaims = new List<(string, string)>
        {
            (CpnucleoClaimTypes.SessionStartedAt, now.ToUnixTimeSeconds().ToString(System.Globalization.CultureInfo.InvariantCulture)),
            (CpnucleoClaimTypes.Subject, item.Id.ToString()),
            (CpnucleoClaimTypes.UserId, item.Id.ToString()),
            (ClaimTypes.NameIdentifier, item.Id.ToString())
        };
        if (tenantId != Guid.Empty)
        {
            sessionClaims.Add((CpnucleoClaimTypes.TenantId, tenantId.ToString()));
            sessionClaims.Add((CpnucleoClaimTypes.TenantSlug, tenantId.ToString()));
        }

        Response.Token = tokenIssuer.Issue(item, sessionClaims, now + TokenIssuer.AccessTokenLifetime);

        Logger.LogInformation("Issued an access token for user {UserId}.", item.Id);
        await Send.OkAsync(Response, cancellationToken);
    }
}
