namespace Cpnucleo.Shared.Queries.ListImpedimento;

public sealed record ListImpedimentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoViewModel>;