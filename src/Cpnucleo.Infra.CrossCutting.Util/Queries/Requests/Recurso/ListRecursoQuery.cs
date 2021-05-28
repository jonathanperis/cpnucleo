using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso
{
    public class ListRecursoQuery : IRequest<ListRecursoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
