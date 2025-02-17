namespace Application.UseCases.AssignmentType.GetAssignmentTypeById;

public sealed record GetAssignmentTypeByIdQuery(Guid Id) : IRequest<GetAssignmentTypeByIdQueryViewModel>;
