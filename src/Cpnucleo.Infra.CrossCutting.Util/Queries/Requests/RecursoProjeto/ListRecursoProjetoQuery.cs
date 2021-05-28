using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto
{
    public class ListRecursoProjetoQuery : IRequest<ListRecursoProjetoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
