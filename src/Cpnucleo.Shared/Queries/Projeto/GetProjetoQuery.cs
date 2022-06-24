namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Projeto;

public record GetProjetoQuery(Guid Id) : BaseQuery, IRequest<GetProjetoViewModel>;