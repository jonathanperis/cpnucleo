namespace Application.UseCases.Impediment.GetImpedimentById;

public sealed record GetImpedimentByIdQuery(Ulid Id) : IRequest<GetImpedimentByIdQueryViewModel>;
