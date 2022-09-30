namespace Cpnucleo.Shared.Queries.Impedimento;

public sealed record ListImpedimentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoViewModel>;