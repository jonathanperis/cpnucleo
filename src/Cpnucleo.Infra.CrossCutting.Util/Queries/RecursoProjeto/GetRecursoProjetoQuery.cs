namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetRecursoProjetoQuery : IRequest<GetRecursoProjetoViewModel>
{
    public Guid Id { get; set; }
}
