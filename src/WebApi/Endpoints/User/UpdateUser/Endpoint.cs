namespace WebApi.Endpoints.User.UpdateUser;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, IPasswordHasher passwordHasher) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/user");
        Description(x => x.WithTags("Users"));
        Policies("UserAdministration");

        Summary(s =>
        {
            s.Summary = "Update an existing user";
            s.Description = "Updates the user identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Checking if an user entity exists with Id: {UserId}", request.Id);
        var item = await dbContext.Users!.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (item is null)
        {
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Updating user entity with Id: {UserId}", request.Id);
        PasswordHash? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            passwordHash = passwordHasher.Hash(request.Password);
        }
        Domain.Entities.User.Update(item, request.Name, request.Login ?? item.Login, passwordHash);

        Logger.LogInformation("Updating entity in repository.");
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Update result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged();

        await Send.OkAsync(Response, cancellationToken);
    }
}
