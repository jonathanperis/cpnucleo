namespace Application.UseCases.Assignment.GetAssignmentById;

public sealed record GetAssignmentByIdQuery(Guid Id) : IRequest<GetAssignmentByIdQueryViewModel>;
