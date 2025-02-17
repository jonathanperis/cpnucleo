namespace Application.UseCases.Project.GetProjectById;

public sealed record GetProjectByIdQuery(Guid Id) : IRequest<GetProjectByIdQueryViewModel>;
