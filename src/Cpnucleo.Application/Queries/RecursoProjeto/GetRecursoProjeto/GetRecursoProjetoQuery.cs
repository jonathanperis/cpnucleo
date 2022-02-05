namespace Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjeto;

public class GetRecursoProjetoQuery : IRequest<GetRecursoProjetoViewModel>
{
    public Guid Id { get; set; }
}
