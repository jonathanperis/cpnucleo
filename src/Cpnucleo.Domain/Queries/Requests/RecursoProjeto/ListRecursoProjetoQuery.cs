using Cpnucleo.Domain.Queries.Responses.RecursoProjeto;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.RecursoProjeto
{
    public class ListRecursoProjetoQuery : IRequest<ListRecursoProjetoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
