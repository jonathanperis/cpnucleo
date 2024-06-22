namespace Application.UseCases.Assignment.GetAssignmentById;

public sealed record GetAssignmentByIdQuery(Ulid Id) : IRequest<GetAssignmentByIdQueryViewModel>;
