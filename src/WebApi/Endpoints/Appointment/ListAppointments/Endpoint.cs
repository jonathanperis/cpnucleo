namespace WebApi.Endpoints.Appointment.ListAppointments;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/appointments");
        Description(x => x.WithTags("Appointments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of appointments";
            s.Description = "Fetches appointments based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all appointments with pagination page {PageNumber}, size {PageSize}", request.Pagination.PageNumber, request.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} appointment records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = response.MapToDto(x => x?.MapToDto());

        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
