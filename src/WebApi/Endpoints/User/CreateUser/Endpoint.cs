namespace WebApi.Endpoints.User.CreateUser;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/user");
        Description(x => x.WithTags("Users"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new user";
            s.Description = "Creates a new user record with the given data and custom Id. Validates uniqueness and returns the created user's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {UserId}", request.Name, request.Id);

            Logger.LogInformation("Checking if an user entity exists with Id: {UserId}", request.Id);
            var itemExists = dbContext.Users!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("User Id conflict for Id: {UserId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new user entity.");
            var newItem = Domain.Entities.User.Create(request.Name, request.Login, request.Password, request.Id);
            Logger.LogInformation("Created new user entity with Id: {UserId}", newItem.Id);

            Logger.LogInformation("Adding user to repository.");
            await dbContext.Users!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching user by Id: {UserId}", newItem.Id);
            var createdItem = await dbContext.Users!.FindAsync(newItem.Id, cancellationToken);

            Response.User = createdItem!.MapToDto();

            Logger.LogInformation("Service completed successfully.");

            await SendOkAsync(Response, cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
