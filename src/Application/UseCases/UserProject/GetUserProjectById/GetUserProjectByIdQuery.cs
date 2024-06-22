namespace Application.UseCases.UserProject.GetUserProjectById;

public sealed record GetUserProjectByIdQuery(Ulid Id) : IRequest<GetUserProjectByIdQueryViewModel>;
