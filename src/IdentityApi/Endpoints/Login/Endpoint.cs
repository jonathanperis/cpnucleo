namespace IdentityApi.Endpoints.Login;

public class Endpoint(IApplicationDbContext dbContext, IPasswordHasher passwordHasher) : Endpoint<Request, Response>
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
        var item = await dbContext.Users!
            .FirstOrDefaultAsync(u => u.Login == req.Login, cancellationToken);

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

        var adminLogins = (Environment.GetEnvironmentVariable("CPNUCLEO_ADMIN_LOGINS") ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var isAdmin = adminLogins.Any(login => string.Equals(login, item.Login, StringComparison.OrdinalIgnoreCase));

        var jwtToken = JwtBearer.CreateToken(o =>
        {
            o.ExpireAt = DateTime.UtcNow.AddMinutes(30);
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