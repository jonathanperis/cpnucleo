namespace Application.UseCases.AssignmentImpediment.GetAssignmentImpedimentById;

public sealed record GetAssignmentImpedimentByIdQuery(Guid Id) : BaseQuery, IRequest<GetAssignmentImpedimentByIdQueryViewModel>;
