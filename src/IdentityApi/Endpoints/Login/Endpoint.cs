namespace IdentityApi.Endpoints.Login;

public class Endpoint(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, IConfiguration configuration) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/login");
        Description(x => x.WithTags("Authentication"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Authenticate user and generate JWT token";
            s.Description =
                "Authenticates the user based on provided credentials and generates a JWT token upon successful authentication.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching user entity with Login: {UserLogin}", req.Login);
        var normalizedLogin = req.Login.Trim().ToLowerInvariant();
        var matches = await dbContext.Users!
            .Where(u => u.Login != null && u.Login.Trim().ToLower() == normalizedLogin)
            .Take(2).ToListAsync(cancellationToken);
        var item = matches.Count == 1 ? matches[0] : null;

        if (item is null)
        {
            Logger.LogWarning("User not found with Login: {UserLogin}", req.Login);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        if (!passwordHasher.Verify(req.Password, item.Password))
        {
            Logger.LogWarning("Invalid password for Login: {UserLogin}", req.Login);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Creating JWT token for user with Login: {UserLogin}", req.Login);

        var tenantId = await (from userProject in dbContext.UserProjects!
                join project in dbContext.Projects! on userProject.ProjectId equals project.Id
                where userProject.UserId == item.Id
                select project.OrganizationId)
            .FirstOrDefaultAsync(cancellationToken);

        var adminLogins = (configuration["CPNUCLEO_ADMIN_LOGINS"] ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var isAdmin = adminLogins.Any(login => string.Equals(login, item.Login, StringComparison.OrdinalIgnoreCase));

        var jwtToken = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey is required.");
            o.Issuer = configuration["Jwt:Issuer"];
            o.Audience = configuration["Jwt:Audience"];
            o.ExpireAt = DateTime.UtcNow.AddMinutes(30);
            o.User.Claims.Add((CpnucleoClaimTypes.SessionStartedAt, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(System.Globalization.CultureInfo.InvariantCulture)));
            o.User.Claims.Add((CpnucleoClaimTypes.Subject, item.Id.ToString()));
            o.User.Claims.Add((CpnucleoClaimTypes.UserId, item.Id.ToString()));
            o.User.Claims.Add((ClaimTypes.NameIdentifier, item.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(item.Login))
            {
                o.User.Claims.Add((CpnucleoClaimTypes.Login, item.Login));
                o.User.Claims.Add((ClaimTypes.Name, item.Login));
            }

            if (tenantId != Guid.Empty)
            {
                var tenantValue = tenantId.ToString();
                o.User.Claims.Add((CpnucleoClaimTypes.TenantId, tenantValue));
                o.User.Claims.Add((CpnucleoClaimTypes.TenantSlug, tenantValue));
            }

            if (isAdmin)
            {
                o.User.Claims.Add((CpnucleoClaimTypes.Admin, "true"));
            }
        });

        Response.Token = jwtToken;
        
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);        
    }
}
