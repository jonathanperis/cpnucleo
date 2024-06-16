namespace Application.UseCases.Project.GetProjectById;

public sealed record GetProjectByIdQuery(Ulid Id) : IRequest<GetProjectByIdQueryViewModel>;
