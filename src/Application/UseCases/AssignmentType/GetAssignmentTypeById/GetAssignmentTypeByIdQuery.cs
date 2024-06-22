namespace Application.UseCases.AssignmentType.GetAssignmentTypeById;

public sealed record GetAssignmentTypeByIdQuery(Ulid Id) : IRequest<GetAssignmentTypeByIdQueryViewModel>;
