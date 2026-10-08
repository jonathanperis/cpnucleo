namespace WebApi.Endpoints.User.GetUserById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/user");
        Group<UserAdministrationGroup>();

        Summary(s =>
        {
            s.Summary = "Retrieve an user by Id";
            s.Description = "Fetches the user matching the provided Id. Returns 404 if not found, otherwise returns the user data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Fetching user entity with Id: {UserId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (item is null)
        {
            Logger.LogWarning("User not found with Id: {UserId}", request.Id);
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserId}", request.Id);
        Response.User = item.MapToDto();

        await Send.OkAsync(Response, cancellationToken);
    }
}
