namespace WebApi.Endpoints.User.GetUserById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/user");
        Description(x => x.WithTags("Users"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an user by Id";
            s.Description = "Fetches the user matching the provided Id. Returns 404 if not found, otherwise returns the user data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching user entity with Id: {UserId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("User not found with Id: {UserId}", request.Id);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserId}", request.Id);
        Response.User = item.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
