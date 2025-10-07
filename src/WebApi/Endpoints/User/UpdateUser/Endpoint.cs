namespace WebApi.Endpoints.User.UpdateUser;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/user");
        Description(x => x.WithTags("Users"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing user";
            s.Description = "Updates the user identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an user entity exists with Id: {UserId}", request.Id);
            var item = await dbContext.Users!.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating user entity with Id: {UserId}", request.Id);
            Domain.Entities.User.Update(item, request.Name, request.Password);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Update result: {Success}", Response.Success);
            Logger.LogInformation("Service completed successfully.");

            await Send.OkAsync(Response, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
