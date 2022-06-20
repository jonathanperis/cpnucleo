namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Projeto;

public record ListProjetoQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListProjetoViewModel>;