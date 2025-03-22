namespace Application.UseCases.UserAssignment.GetUserAssignmentById;

public sealed record GetUserAssignmentByIdQuery(Guid Id) : BaseQuery, IRequest<GetUserAssignmentByIdQueryViewModel>;
