namespace Application.UseCases.UserAssignment.GetUserAssignmentById;

public sealed record GetUserAssignmentByIdQuery(Ulid Id) : IRequest<GetUserAssignmentByIdQueryViewModel>;
