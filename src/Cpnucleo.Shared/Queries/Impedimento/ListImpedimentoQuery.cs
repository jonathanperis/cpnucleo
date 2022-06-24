using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Impedimento;

public record ListImpedimentoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListImpedimentoViewModel>;