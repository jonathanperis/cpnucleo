namespace Application.UseCases.Impediment.GetImpedimentById;

public sealed record GetImpedimentByIdQuery(Guid Id) : IRequest<GetImpedimentByIdQueryViewModel>;
