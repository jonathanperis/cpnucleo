namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Impedimento;

public record ListImpedimentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoViewModel>;