namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Impedimento;

public record GetImpedimentoQuery(Guid Id) : BaseQuery, IRequest<GetImpedimentoViewModel>;