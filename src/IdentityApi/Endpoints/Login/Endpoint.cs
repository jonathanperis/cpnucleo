namespace IdentityApi.Endpoints.Login;

public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/login");
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
            .FirstOrDefaultAsync(u => u.Login == req.Login && u.Password == req.Password, cancellationToken);

        if (item is null)
        {
            Logger.LogWarning("User not found with Login: {UserLogin}", req.Login);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Creating JWT token for user with Login: {UserLogin}", req.Login);
        
        var jwtToken = JwtBearer.CreateToken(
            o =>
            {
                // o.User.Roles.Add("Manager", "Auditor");
                // o.User.Claims.Add(("UserName", req.Username));
                // o.User["UserId"] = "001"; //indexer based claim setting
            });     
        
        Response.Token = jwtToken;
        
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);        
    }
}