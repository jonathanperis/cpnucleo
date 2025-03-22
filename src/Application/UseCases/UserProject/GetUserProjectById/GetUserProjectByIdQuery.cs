namespace Application.UseCases.UserProject.GetUserProjectById;

public sealed record GetUserProjectByIdQuery(Guid Id) : BaseQuery, IRequest<GetUserProjectByIdQueryViewModel>;
